# SmartCharging
The application is used in order to calculate the best way to charge electric vehicles.
It uses Greedy Algorithm as the most optimal way to resolve the problem and its complexity is O(n).
The Controller consists of several parts:
1. JSONConverter is responsible for JSON Serialization and Deserialization.
2. IOHandler is working with Reading and Writing data from/in the file.
3. Engine is a hub where all the calculations are made.

The calculation is made in CalculateCharging() method and consists of several steps:
1. Calculates applicable Tariffs from Starting Date till Leaving Date in seconds. 
It also takes into consideration if the Leaving Date is on the next day from Starting Date.
2. Calculates Direct Charging and Smart Charging time in seconds
3. Sorts Tariffs by Date and calculates Direct Charging
4. Sort Tariffs by Price and calculates Smart Charging
5. Calculates the Output result based on consumed energy from Tariffs

CalculateRelevantTariffs() is working in the following way:
1. Gets all tariffs for the day of Starting Date and Leaving Date as well if the days are different.
2. Checks if the Starting Date more than Tariffs End Time or Leaving Date is less than Tariffs Start Time.
If so, algorithm ignores them.
3. In case when Starting Date more than Tariffs Start Time, we update Start Time for Tariff and set it to Starting Time.
We are doing the same with Leaving Date and End Time.

The method Charge() goes through all the Tariffs.
1. It compares if Tariffs Left Time in seconds is less than Charging Time (the time in seconds we have to charge vehicle).
If so, we substart from Charging Time Tariffs Time and put Tariffs time to 0, considering the fact we used it fully.
2. If Tariffs Time is more than Charging Time, we do invers and return from the method.
