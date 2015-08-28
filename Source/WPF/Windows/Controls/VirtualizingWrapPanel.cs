using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace CCSWE.Windows.Controls
{
    //TODO: Clean this up...
    /// <summary>
    /// Code modified from http://www.codeproject.com/Articles/75847/Virtualizing-WrapPanel
    /// Possible bug fix: http://www.codeproject.com/Articles/75847/Virtualizing-WrapPanel?msg=5066075#xx5066075xx
    /// </summary>
    public class VirtualizingWrapPanel : VirtualizingPanel, IScrollInfo
    {
        #region Constructor
        public VirtualizingWrapPanel()
        {
            CanHorizontallyScroll = false;
            CanVerticallyScroll = false;
        }        
        #endregion

        #region Private Fields
        private WrapPanelAbstraction _abstractPanel;
        private UIElementCollection _children;
        private Size _childSize;
        private Size _extent = new Size(0, 0);
        private int _firstIndex = 0;
        private IItemContainerGenerator _generator;
        private ItemsControl _itemsControl;
        private Point _offset = new Point(0, 0);
        private Size _pixelMeasuredViewport = new Size(0, 0);
        private readonly Dictionary<UIElement, Rect> _realizedChildLayout = new Dictionary<UIElement, Rect>();
        private Size _viewport = new Size(0, 0);
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register("ItemHeight", typeof(double), typeof(VirtualizingWrapPanel), new UIPropertyMetadata(double.PositiveInfinity), ValidateItemSize);
        public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register("ItemWidth", typeof(double), typeof(VirtualizingWrapPanel), new UIPropertyMetadata(double.PositiveInfinity), ValidateItemSize);
        public static readonly DependencyProperty OrientationProperty = StackPanel.OrientationProperty.AddOwner(typeof(VirtualizingWrapPanel), new UIPropertyMetadata(Orientation.Horizontal));
        #endregion

        #region Public Properties
        private Size ChildSlotSize
        {
            get { return new Size(ItemWidth, ItemHeight); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        #endregion

        #region Private Methods
        private void CleanUpItems(int minDesiredGenerated, int maxDesiredGenerated)
        {
            for (var i = _children.Count - 1; i >= 0; i--)
            {
                var childGeneratorPos = new GeneratorPosition(i, 0);
                var itemIndex = _generator.IndexFromGeneratorPosition(childGeneratorPos);
                if (itemIndex < minDesiredGenerated || itemIndex > maxDesiredGenerated)
                {
                    if (itemIndex >= 0)
                    {
                        _generator.Remove(childGeneratorPos, 1);
                    }
                    RemoveInternalChildRange(i, 1);
                }
            }
        }

        private void ComputeExtentAndViewport(Size pixelMeasuredViewportSize, int visibleSections)
        {
            if (Orientation == Orientation.Horizontal)
            {
                _viewport.Height = visibleSections;
                _viewport.Width = pixelMeasuredViewportSize.Width;
            }
            else
            {
                _viewport.Width = visibleSections;
                _viewport.Height = pixelMeasuredViewportSize.Height;
            }

            if (Orientation == Orientation.Horizontal)
            {
                _extent.Height = _abstractPanel.SectionCount + ViewportHeight - 1;

            }
            else
            {
                _extent.Width = _abstractPanel.SectionCount + ViewportWidth - 1;
            }

            ScrollOwner.InvalidateScrollInfo();
        }

        private int GetFirstVisibleIndex()
        {
            var section = GetFirstVisibleSection();

            //TODO: Quick hack. Need to dig into why this is null
            if (_abstractPanel == null)
            {
                return 0;
            }

            var item = _abstractPanel.FirstOrDefault(x => x.Section == section);

            if (item != null)
            {
                return item._index;
            }

            return 0;
        }

        private int GetFirstVisibleSection()
        {
            int section;

            //TODO: Quick hack. Need to dig into why this is null
            if (_abstractPanel == null)
            {
                return 0;
            }

            var maxSection = _abstractPanel.Max(x => x.Section);

            if (Orientation == Orientation.Horizontal)
            {
                section = (int)_offset.Y;
            }
            else
            {
                section = (int)_offset.X;
            }

            if (section > maxSection)
            {
                section = maxSection;
            }

            return section;
        }

        private int GetLastSectionClosestIndex(int itemIndex)
        {
            var abstractItem = _abstractPanel[itemIndex];
            if (abstractItem.Section > 0)
            {
                var ret = _abstractPanel.Where(x => x.Section == abstractItem.Section - 1).OrderBy(x => Math.Abs(x.SectionIndex - abstractItem.SectionIndex)).First();
                return ret._index;
            }
            else
            {
                return itemIndex;
            }
        }

        private int GetNextSectionClosestIndex(int itemIndex)
        {
            var abstractItem = _abstractPanel[itemIndex];
            if (abstractItem.Section < _abstractPanel.SectionCount - 1)
            {
                var ret = _abstractPanel.Where(x => x.Section == abstractItem.Section + 1).OrderBy(x => Math.Abs(x.SectionIndex - abstractItem.SectionIndex)).First();
                return ret._index;
            }
            else
            {
                return itemIndex;
            }
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (_viewport.Width != 0)
            {
                var firstIndex = _firstIndex;

                _abstractPanel = null;

                MeasureOverride(_viewport);
                //TODO: Isn't this wrong? 
                SetFirstRowViewItemIndex(_firstIndex);

                _firstIndex = firstIndex;
            }
        }

        //private void RemoveChildRange(GeneratorPosition position, int itemCount, int itemUICount)
        //{
        //    if (IsItemsHost)
        //    {
        //        UIElementCollection children = InternalChildren;
        //        int pos = position.Index;
        //        if (position.Offset > 0)
        //        {
        //            // An item is being removed after the one at the index
        //            pos++;
        //        }

        //        if (pos < children.Count)
        //        {
        //            int uiCount = itemUICount;
        //            Debug.Assert((itemCount == itemUICount) || (itemUICount == 0), "Both ItemUICount and ItemCount should be equal or ItemUICount should be 0.");
        //            if (uiCount > 0)
        //            {
        //                RemoveInternalChildRange(children, pos, uiCount);

        //                if (IsVirtualizing && InRecyclingMode)
        //                {
        //                    _realizedChildren.RemoveRange(pos, uiCount);
        //                }
        //            }
        //        }
        //    }
        //}

        private void ResetScrollInfo()
        {
            _offset.X = 0;
            _offset.Y = 0;
        }

        //NOTE: Was public
        private void SetFirstRowViewItemIndex(int index)
        {
            SetVerticalOffset((index) / Math.Floor((_viewport.Width) / _childSize.Width));
            SetHorizontalOffset((index) / Math.Floor((_viewport.Height) / _childSize.Height));
        }

        private static bool ValidateItemSize(object value)
        {
            return ((double) value > 0d);
        }
        #endregion

        #region Protected Methods
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (_children != null)
            {
                foreach (UIElement child in _children)
                {
                    if (_realizedChildLayout.ContainsKey(child))
                    {
                        var layoutInfo = _realizedChildLayout[child];
                        child.Arrange(layoutInfo);
                    }
                }
            }
            return finalSize;
        }

        /// <summary>
        /// Used by the ScrollIntoView method
        /// </summary>
        /// <param name="index">The index of the item to bring into view</param>
        protected override void BringIndexIntoView(int index)
        {
            SetFirstRowViewItemIndex(index);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (_itemsControl == null || _itemsControl.Items.Count == 0)
            {
                return availableSize;
            }

            if (_abstractPanel == null)
            {
                _abstractPanel = new WrapPanelAbstraction(_itemsControl.Items.Count);
            }


            _pixelMeasuredViewport = availableSize;

            _realizedChildLayout.Clear();

            var realizedFrameSize = availableSize;

            var itemCount = _itemsControl.Items.Count;
            var firstVisibleIndex = GetFirstVisibleIndex();

            var startPos = _generator.GeneratorPositionFromIndex(firstVisibleIndex);

            var childIndex = (startPos.Offset == 0) ? startPos.Index : startPos.Index + 1;
            var childSlotSize = ChildSlotSize;
            var current = firstVisibleIndex;
            var visibleSections = 1;

            using (_generator.StartAt(startPos, GeneratorDirection.Forward, true))
            {
                var stop = false;
                var isHorizontal = Orientation == Orientation.Horizontal;

                double currentX = 0;
                double currentY = 0;
                double maxItemSize = 0;

                var currentSection = GetFirstVisibleSection();

                while (current < itemCount)
                {
                    bool newlyRealized;

                    // Get or create the child                    
                    var child = _generator.GenerateNext(out newlyRealized) as UIElement;

                    if (child == null)
                    {
                        throw new InvalidOperationException("'child' cannot be null");
                    }

                    if (newlyRealized)
                    {
                        // Figure out if we need to insert the child at the end or somewhere in the middle
                        if (childIndex >= _children.Count)
                        {
                            AddInternalChild(child);
                        }
                        else
                        {
                            base.InsertInternalChild(childIndex, child);
                        }

                        _generator.PrepareItemContainer(child);

                        child.Measure(childSlotSize);
                    }
                    else
                    {
                        if ((!childSlotSize.Height.Equals(double.PositiveInfinity) && !child.DesiredSize.Height.Equals(childSlotSize.Height)) || (!childSlotSize.Width.Equals(double.PositiveInfinity) && !child.DesiredSize.Width.Equals(childSlotSize.Width)))
                        {
                            // Re-measure if the ChildSlotSize has changed
                            child.Measure(childSlotSize);
                        }

                        // ReSharper disable once PossibleUnintendedReferenceComparison
                        // The child has already been created, let's be sure it's in the right spot
                        Debug.Assert(child == _children[childIndex], "Wrong child was generated");
                    }

                    _childSize = child.DesiredSize;
                    var childRect = new Rect(new Point(currentX, currentY), _childSize);

                    if (isHorizontal)
                    {
                        maxItemSize = Math.Max(maxItemSize, childRect.Height);

                        if (childRect.Right > realizedFrameSize.Width) //wrap to a new line
                        {
                            currentY = currentY + maxItemSize;
                            currentX = 0;
                            maxItemSize = childRect.Height;
                            childRect.X = currentX;
                            childRect.Y = currentY;
                            currentSection++;
                            visibleSections++;
                        }

                        if (currentY > realizedFrameSize.Height)
                        {
                            stop = true;
                        }

                        currentX = childRect.Right;
                    }
                    else
                    {
                        maxItemSize = Math.Max(maxItemSize, childRect.Width);

                        if (childRect.Bottom > realizedFrameSize.Height) //wrap to a new column
                        {
                            currentX = currentX + maxItemSize;
                            currentY = 0;
                            maxItemSize = childRect.Width;
                            childRect.X = currentX;
                            childRect.Y = currentY;
                            currentSection++;
                            visibleSections++;
                        }

                        if (currentX > realizedFrameSize.Width)
                        {
                            stop = true;
                        }

                        currentY = childRect.Bottom;
                    }

                    _realizedChildLayout.Add(child, childRect);
                    _abstractPanel.SetItemSection(current, currentSection);

                    if (stop)
                    {
                        break;
                    }

                    current++;
                    childIndex++;
                }
            }

            CleanUpItems(firstVisibleIndex, current - 1);
            ComputeExtentAndViewport(availableSize, visibleSections);

            return availableSize;
        }

        protected override void OnItemsChanged(object sender, ItemsChangedEventArgs args)
        {
            //Debug.WriteLine("OnItemsChanged: " + args.Action + " - " + args.ItemCount + " - " + args.ItemUICount + " - " + args.OldPosition + " - " + args.Position);
            _abstractPanel = null;


            //TODO: This is sort of copied from the VirtualizingStackPanel code. Assuming the other action cases need to be handled as well
            if (args.Action == NotifyCollectionChangedAction.Remove)
            {
                RemoveInternalChildRange(args.Position.Index, args.ItemUICount);
            }

            base.OnItemsChanged(sender, args);

            ResetScrollInfo();
        }

        protected override void OnInitialized(EventArgs e)
        {
            SizeChanged += OnSizeChanged;
            
            base.OnInitialized(e);

            _itemsControl = ItemsControl.GetItemsOwner(this);
            //TODO: Why not just use InternalChildren everywhere?
            _children = InternalChildren;
            //TODO: Why not just use ItemContainerGenerator everywhere?
            _generator = ItemContainerGenerator;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    {
                        NavigateDown();
                        e.Handled = true;
                        break;
                    }
                case Key.Left:
                    {
                        NavigateLeft();
                        e.Handled = true;
                        break;
                    }
                case Key.Right:
                    {
                        NavigateRight();
                        e.Handled = true;
                        break;
                    }
                case Key.Up:
                    {
                        NavigateUp();
                        e.Handled = true;
                        break;
                    }
                default:
                    {
                        base.OnKeyDown(e);
                        break;
                    }
            }
        }
        #endregion

        #region Public 












        private void NavigateDown()
        {
            var gen = _generator.GetItemContainerGeneratorForPanel(this);
            var selected = (UIElement)Keyboard.FocusedElement;
            var itemIndex = gen.IndexFromContainer(selected);
            var depth = 0;
            while (itemIndex == -1)
            {
                selected = (UIElement)VisualTreeHelper.GetParent(selected);
                itemIndex = gen.IndexFromContainer(selected);
                depth++;
            }
            DependencyObject next = null;
            if (Orientation == Orientation.Horizontal)
            {
                var nextIndex = GetNextSectionClosestIndex(itemIndex);
                next = gen.ContainerFromIndex(nextIndex);
                while (next == null)
                {
                    SetVerticalOffset(VerticalOffset + 1);
                    UpdateLayout();
                    next = gen.ContainerFromIndex(nextIndex);
                }
            }
            else
            {
                if (itemIndex == _abstractPanel._itemCount - 1)
                    return;
                next = gen.ContainerFromIndex(itemIndex + 1);
                while (next == null)
                {
                    SetHorizontalOffset(HorizontalOffset + 1);
                    UpdateLayout();
                    next = gen.ContainerFromIndex(itemIndex + 1);
                }
            }
            while (depth != 0)
            {
                next = VisualTreeHelper.GetChild(next, 0);
                depth--;
            }
            (next as UIElement).Focus();
        }

        private void NavigateLeft()
        {
            var gen = _generator.GetItemContainerGeneratorForPanel(this);

            var selected = (UIElement)Keyboard.FocusedElement;
            var itemIndex = gen.IndexFromContainer(selected);
            var depth = 0;
            while (itemIndex == -1)
            {
                selected = (UIElement)VisualTreeHelper.GetParent(selected);
                itemIndex = gen.IndexFromContainer(selected);
                depth++;
            }
            DependencyObject next = null;
            if (Orientation == Orientation.Vertical)
            {
                var nextIndex = GetLastSectionClosestIndex(itemIndex);
                next = gen.ContainerFromIndex(nextIndex);
                while (next == null)
                {
                    SetHorizontalOffset(HorizontalOffset - 1);
                    UpdateLayout();
                    next = gen.ContainerFromIndex(nextIndex);
                }
            }
            else
            {
                if (itemIndex == 0)
                    return;
                next = gen.ContainerFromIndex(itemIndex - 1);
                while (next == null)
                {
                    SetVerticalOffset(VerticalOffset - 1);
                    UpdateLayout();
                    next = gen.ContainerFromIndex(itemIndex - 1);
                }
            }
            while (depth != 0)
            {
                next = VisualTreeHelper.GetChild(next, 0);
                depth--;
            }
            (next as UIElement).Focus();
        }

        private void NavigateRight()
        {
            var gen = _generator.GetItemContainerGeneratorForPanel(this);
            var selected = (UIElement)Keyboard.FocusedElement;
            var itemIndex = gen.IndexFromContainer(selected);
            var depth = 0;
            while (itemIndex == -1)
            {
                selected = (UIElement)VisualTreeHelper.GetParent(selected);
                itemIndex = gen.IndexFromContainer(selected);
                depth++;
            }
            DependencyObject next = null;
            if (Orientation == Orientation.Vertical)
            {
                var nextIndex = GetNextSectionClosestIndex(itemIndex);
                next = gen.ContainerFromIndex(nextIndex);
                while (next == null)
                {
                    SetHorizontalOffset(HorizontalOffset + 1);
                    UpdateLayout();
                    next = gen.ContainerFromIndex(nextIndex);
                }
            }
            else
            {
                if (itemIndex == _abstractPanel._itemCount - 1)
                    return;
                next = gen.ContainerFromIndex(itemIndex + 1);
                while (next == null)
                {
                    SetVerticalOffset(VerticalOffset + 1);
                    UpdateLayout();
                    next = gen.ContainerFromIndex(itemIndex + 1);
                }
            }
            while (depth != 0)
            {
                next = VisualTreeHelper.GetChild(next, 0);
                depth--;
            }
            (next as UIElement).Focus();
        }

        private void NavigateUp()
        {
            var gen = _generator.GetItemContainerGeneratorForPanel(this);
            var selected = (UIElement)Keyboard.FocusedElement;
            var itemIndex = gen.IndexFromContainer(selected);
            var depth = 0;
            while (itemIndex == -1)
            {
                selected = (UIElement)VisualTreeHelper.GetParent(selected);
                itemIndex = gen.IndexFromContainer(selected);
                depth++;
            }
            DependencyObject next = null;
            if (Orientation == Orientation.Horizontal)
            {
                var nextIndex = GetLastSectionClosestIndex(itemIndex);
                next = gen.ContainerFromIndex(nextIndex);
                while (next == null)
                {
                    SetVerticalOffset(VerticalOffset - 1);
                    UpdateLayout();
                    next = gen.ContainerFromIndex(nextIndex);
                }
            }
            else
            {
                if (itemIndex == 0)
                    return;
                next = gen.ContainerFromIndex(itemIndex - 1);
                while (next == null)
                {
                    SetHorizontalOffset(HorizontalOffset - 1);
                    UpdateLayout();
                    next = gen.ContainerFromIndex(itemIndex - 1);
                }
            }
            while (depth != 0)
            {
                next = VisualTreeHelper.GetChild(next, 0);
                depth--;
            }
            (next as UIElement).Focus();
        }


        #endregion

        #region Override







        #endregion

        #region IScrollInfo Members
        #region Public Properties
        public bool CanHorizontallyScroll { get; set; }

        public bool CanVerticallyScroll { get; set; }

        public double ExtentHeight
        {
            get { return _extent.Height; }
        }

        public double ExtentWidth
        {
            get { return _extent.Width; }
        }

        public double HorizontalOffset
        {
            get { return _offset.X; }
        }

        public ScrollViewer ScrollOwner { get; set; }

        public double VerticalOffset
        {
            get { return _offset.Y; }
        }

        public double ViewportHeight
        {
            get { return _viewport.Height; }
        }

        public double ViewportWidth
        {
            get { return _viewport.Width; }
        }
        #endregion

        #region Public Methods
        public void LineDown()
        {
            if (Orientation == Orientation.Vertical)
                SetVerticalOffset(VerticalOffset + 20);
            else
                SetVerticalOffset(VerticalOffset + 1);
        }

        public void LineLeft()
        {
            if (Orientation == Orientation.Horizontal)
                SetHorizontalOffset(HorizontalOffset - 20);
            else
                SetHorizontalOffset(HorizontalOffset - 1);
        }

        public void LineRight()
        {
            if (Orientation == Orientation.Horizontal)
                SetHorizontalOffset(HorizontalOffset + 20);
            else
                SetHorizontalOffset(HorizontalOffset + 1);
        }

        public void LineUp()
        {
            if (Orientation == Orientation.Vertical)
                SetVerticalOffset(VerticalOffset - 20);
            else
                SetVerticalOffset(VerticalOffset - 1);
        }

        public Rect MakeVisible(Visual visual, Rect rectangle)
        {
            var gen = (ItemContainerGenerator)_generator.GetItemContainerGeneratorForPanel(this);
            var element = (UIElement)visual;
            var itemIndex = gen.IndexFromContainer(element);
            while (itemIndex == -1)
            {
                element = (UIElement)VisualTreeHelper.GetParent(element);
                itemIndex = gen.IndexFromContainer(element);
            }

            var elementRect = _realizedChildLayout[element];

            //TODO: Why is this null??
            if (_abstractPanel == null)
            {
                return elementRect;
            }

            var section = _abstractPanel[itemIndex].Section;
            if (Orientation == Orientation.Horizontal)
            {
                var viewportHeight = _pixelMeasuredViewport.Height;
                if (elementRect.Bottom > viewportHeight)
                    _offset.Y += 1;
                else if (elementRect.Top < 0)
                    _offset.Y -= 1;
            }
            else
            {
                var viewportWidth = _pixelMeasuredViewport.Width;
                if (elementRect.Right > viewportWidth)
                    _offset.X += 1;
                else if (elementRect.Left < 0)
                    _offset.X -= 1;
            }
            InvalidateMeasure();
            return elementRect;
        }

        public void MouseWheelDown()
        {
            PageDown();
        }

        public void MouseWheelLeft()
        {
            PageLeft();
        }

        public void MouseWheelRight()
        {
            PageRight();
        }

        public void MouseWheelUp()
        {
            PageUp();
        }

        public void PageDown()
        {
            SetVerticalOffset(VerticalOffset + _viewport.Height * 0.8);
        }

        public void PageLeft()
        {
            SetHorizontalOffset(HorizontalOffset - _viewport.Width * 0.8);
        }

        public void PageRight()
        {
            SetHorizontalOffset(HorizontalOffset + _viewport.Width * 0.8);
        }

        public void PageUp()
        {
            SetVerticalOffset(VerticalOffset - _viewport.Height * 0.8);
        }


        public void SetHorizontalOffset(double offset)
        {
            if (offset < 0 || _viewport.Width >= _extent.Width)
            {
                offset = 0;
            }
            else
            {
                if (offset + _viewport.Width >= _extent.Width)
                {
                    offset = _extent.Width - _viewport.Width;
                }
            }

            _offset.X = offset;

            if (ScrollOwner != null)
            {
                ScrollOwner.InvalidateScrollInfo();
            }

            InvalidateMeasure();

            _firstIndex = GetFirstVisibleIndex();
        }

        public void SetVerticalOffset(double offset)
        {
            Debug.WriteLine("SetVerticalOffset(double " + offset + ")");
            if (offset < 0 || _viewport.Height >= _extent.Height)
            {
                offset = 0;
            }
            else
            {
                if (offset + _viewport.Height >= _extent.Height)
                {
                    offset = _extent.Height - _viewport.Height;
                }
            }

            _offset.Y = offset;

            if (ScrollOwner != null)
            {
                ScrollOwner.InvalidateScrollInfo();
            }

            //_trans.Y = -offset;

            InvalidateMeasure();

            _firstIndex = GetFirstVisibleIndex();
        }
        
        #endregion
        #endregion

        #region helper data structures

        class ItemAbstraction
        {
            public ItemAbstraction(WrapPanelAbstraction panel, int index)
            {
                _panel = panel;
                _index = index;
            }

            WrapPanelAbstraction _panel;

            public readonly int _index;

            int _sectionIndex = -1;
            public int SectionIndex
            {
                get
                {
                    if (_sectionIndex == -1)
                    {
                        return _index % _panel._averageItemsPerSection - 1;
                    }
                    return _sectionIndex;
                }
                set
                {
                    if (_sectionIndex == -1)
                        _sectionIndex = value;
                }
            }

            int _section = -1;
            public int Section
            {
                get
                {
                    if (_section == -1)
                    {
                        return _index / _panel._averageItemsPerSection;
                    }
                    return _section;
                }
                set
                {
                    if (_section == -1)
                        _section = value;
                }
            }
        }

        class WrapPanelAbstraction : IEnumerable<ItemAbstraction>
        {
            public WrapPanelAbstraction(int itemCount)
            {
                var items = new List<ItemAbstraction>(itemCount);
                for (var i = 0; i < itemCount; i++)
                {
                    var item = new ItemAbstraction(this, i);
                    items.Add(item);
                }

                Items = new ReadOnlyCollection<ItemAbstraction>(items);
                _averageItemsPerSection = itemCount;
                _itemCount = itemCount;
            }

            public readonly int _itemCount;
            public int _averageItemsPerSection;
            private int _currentSetSection = -1;
            private int _currentSetItemIndex = -1;
            private int _itemsInCurrentSecction = 0;
            private object _syncRoot = new object();

            public int SectionCount
            {
                get
                {
                    var ret = _currentSetSection + 1;
                    if (_currentSetItemIndex + 1 < Items.Count)
                    {
                        var itemsLeft = Items.Count - _currentSetItemIndex;
                        ret += itemsLeft / _averageItemsPerSection + 1;
                    }
                    return ret;
                }
            }

            private ReadOnlyCollection<ItemAbstraction> Items { get; set; }

            public void SetItemSection(int index, int section)
            {
                lock (_syncRoot)
                {
                    if (section <= _currentSetSection + 1 && index == _currentSetItemIndex + 1)
                    {
                        _currentSetItemIndex++;
                        Items[index].Section = section;
                        if (section == _currentSetSection + 1)
                        {
                            _currentSetSection = section;
                            if (section > 0)
                            {
                                _averageItemsPerSection = (index) / (section);
                            }
                            _itemsInCurrentSecction = 1;
                        }
                        else
                            _itemsInCurrentSecction++;
                        Items[index].SectionIndex = _itemsInCurrentSecction - 1;
                    }
                }
            }

            public ItemAbstraction this[int index]
            {
                get { return Items[index]; }
            }

            #region IEnumerable<ItemAbstraction> Members

            public IEnumerator<ItemAbstraction> GetEnumerator()
            {
                return Items.GetEnumerator();
            }

            #endregion

            #region IEnumerable Members

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion
        }

        #endregion

    }
}
