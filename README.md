[![Build status](https://ci.appveyor.com/api/projects/status/jdqirp46kppcw2jq/branch/master?svg=true)](https://ci.appveyor.com/project/CoryCharlton/ccswe-libraries/branch/master)

# CCSWE Libraries

Just a bunch of C# .NET classes and extension methods I find useful.

If you find this code useful please consider [donating](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=ECGSEZ36LV6QU) to support my efforts.

## CCSWE.Core

* `ConsumerThreadPool<T>` - Provides a specialized thread pool to process items from a `BlockingCollection<T>`
* `SynchronizedObservableCollection<T>` - A thread safe implementation of `ObservableCollection<T>`
* `ThreadSafeQueue<T>` - A thread safe implementation of `Queue<T>`
* Miscellaneous extension methods

## CCSWE.Native

Native Windows API interop declarations

* Gdi32
* Shell32
* User32

## CCSWE.WPF

* `AutoScrollListBox`
* `BusySpinner`
* `VirtualizingWrapPanel`
