# PersistentDb

``` ini
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1645 (21H1/May2021Update)
Intel Core i5-7200U CPU 2.50GHz (Kaby Lake), 1 CPU, 4 logical and 2 physical cores
.NET SDK=6.0.101
  [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
  DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
```

|              Type |                     Method |           Mean |         Error |        StdDev |         Median |            P95 |  Ratio | RatioSD |
|------------------ |--------------------------- |---------------:|--------------:|--------------:|---------------:|---------------:|-------:|--------:|
| FileBasedDatabase |         DirectConstruction |   145,252.4 ns |     870.62 ns |     771.78 ns |   145,245.0 ns |   146,510.7 ns |  1.000 |    0.00 |
|  InMemoryDatabase |         DirectConstruction | 1,700,644.7 ns | 302,108.72 ns | 861,933.18 ns | 1,189,800.0 ns | 3,638,700.0 ns | 11.495 |    5.86 |
|  NoDatabaseAccess |         DirectConstruction |       401.6 ns |       2.10 ns |       1.97 ns |       400.5 ns |       404.5 ns |  0.003 |    0.00 |
| FileBasedDatabase | InvokeReflectedConstructor |   144,584.0 ns |   1,293.92 ns |   1,210.33 ns |   144,324.4 ns |   146,665.4 ns |  0.996 |    0.01 |
|  InMemoryDatabase | InvokeReflectedConstructor | 1,229,829.5 ns |  71,701.57 ns | 185,084.66 ns | 1,163,250.0 ns | 1,558,890.0 ns |  8.745 |    1.88 |
|  NoDatabaseAccess | InvokeReflectedConstructor |       592.2 ns |       3.53 ns |       3.30 ns |       591.6 ns |       597.9 ns |  0.004 |    0.00 |
| FileBasedDatabase |             FullReflection |   145,462.8 ns |     621.72 ns |     551.14 ns |   145,369.8 ns |   146,256.4 ns |  1.001 |    0.01 |
|  InMemoryDatabase |             FullReflection | 1,311,232.1 ns | 121,493.15 ns | 320,061.56 ns | 1,181,500.0 ns | 2,285,600.0 ns |  8.306 |    0.95 |
|  NoDatabaseAccess |             FullReflection |       803.4 ns |       2.84 ns |       2.37 ns |       803.7 ns |       806.6 ns |  0.006 |    0.00 |
