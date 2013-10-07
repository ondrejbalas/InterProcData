InterProcData
=============

This library is to help with inter-process communication within a single machine.

Usage of this library assumes that you have two or more processes - a host process and one or more consumers.

Usage Example - Host
-------------------------

```C#

List<string> sharedData = Enumerable.Range(1, 100000).Select(x => string.Format("Item #{0}", x)).ToList();

using (var host = new DataMap<List<string>>(sharedData))
{
    host.StartHost("someName");
    Console.WriteLine("Hosting.. press enter to exit");
    Console.ReadLine();
}

```

Usage Example - Consumer
-------------------------

```C#

var obj = DataMap<List<string>>.GetHostedData("someName");

Console.WriteLine("Loaded {0} objects" + obj.Count);

Console.WriteLine("Press enter to exit");
Console.ReadLine();

```