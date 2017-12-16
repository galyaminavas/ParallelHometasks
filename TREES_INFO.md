Fine synchronization through locks.

Runtime comparison for Sequential and Parallel trees on random tests:

|                      | 100.000 elements |	1.000.000 elements | 10.000.000 elements (delete 1.000.000 elements) |
| -----                | ---------        | ----               | ------                                          |
| Sequantial insertion | 00.1654 s        | 02.2947 s          | 29.2800 s                                       | 
| Sequential search    | 00.1369 s        | 01.5219 s          | 18.4358 s                                       | 
| Sequential deletion  | 00.1421 s        | 01.5441 s          | 02.1468 s                                       | 
| Parallel insertion   | 00.1481 s        | 01.2680 s          | 13.6236 s                                       | 
| Parallel search      | 00.0662 s        | 00.6362 s          | 06.5658 s                                       |
| Parallel deletion    | 00.0729 s        | 00.6172 s          | 00.7160 s                                       | 
