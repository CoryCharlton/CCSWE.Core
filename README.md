[![Build status](https://ci.appveyor.com/api/projects/status/gynqs96pxgjkxskl?svg=true)](https://ci.appveyor.com/project/CoryCharlton/ccswe-core)

# CCSWE.Core

Just a bunch of C# .NET classes and extension methods I find useful.

If you find this code useful please consider [donating](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=ECGSEZ36LV6QU) to support my efforts.

## Features

* `AppSettings` - Helper class for retrieving values from `ConfigurationManager.AppSettings `.
* `ConsumerThreadPool<T>` - Provides a specialized thread pool to process items from a `BlockingCollection<T>`.
* `Ensure` - A helper class for parameter validation.
* `OperationTracker` - A helper class to track the number of operations executing.
* `SynchronizedObservableCollection<T>` - A thread safe implementation of `ObservableCollection<T>`.
* `ThreadSafeQueue<T>` - A thread safe implementation of `Queue<T>`.
* Miscellaneous extension methods (`DateTime`, `DirectoryInfo`, `FileInfo`, `TimeSpan`)


# CCSWE.Native

Moved to https://github.com/CoryCharlton/CCSWE.Native

# CCSWE.WPF

Moved to https://github.com/CoryCharlton/CCSWE.WPF