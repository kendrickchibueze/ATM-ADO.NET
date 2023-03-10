# Welcome to the Talent Bank ATM Machine with ADO.NETβπ€πππππ
This is  an ATM console application built with C#π.It is designed using object oriented programming(OOP) construct and SQL database. An explicit look at our Entity relationship diagram for my database looks like this:

 ![](https://github.com/kendrickchibueze/-Modern-Node-on-AWS/blob/main/aws-images/Screenshot%20(499).png?raw=true)

## Usageπ
* Clone the repository: ``` git clone https://github.com/kendrickchibueze/ATM-ADO.NET.git```

* Copy and paste your DataSource name into the  connection string  of the App.Config file.

A pragmatic run of our executable assembly looks like this:

![](https://raw.githubusercontent.com/kendrickchibueze/-Modern-Node-on-AWS/5d6752d563ac41bcdf4c1419a5337a4dcae2cbf4/aws-images/Screenshot%20(395).png)

## Database ππ€·ββοΈ
A pragmatic Look at our BankAccount table  for the purpose of Login looks like this:

![](https://raw.githubusercontent.com/kendrickchibueze/-Modern-Node-on-AWS/36199e632d149477b9c498dab8ad25020c0a5670/Screenshot%20(521).png)

## Software Development Summaryπππ
* **Technology**: C#π
* **Framework**: .NET6
* **Project Type**: Console
* **IDE**: Visual Studio (Version 2022)
* **Paradigm or pattern of programming**: Object-Oriented Programming (OOP)
* **DataBase**: I seeded data into  tables in the database.An SQL database  and query are used on purpose for this Atm project.

 ## ATM Basic Features / Use Cases πππ:
 * Check account balance
 * Place deposit
 * Make withdraw
 * Check card number and pin against bank account list object (Note: An SQL database is used for this project)
 * View All transactions
 * Make transfer (Transfer within the same bank but different account number)
 ## Logic
* **Business Rules** π€·ββοΈ:
User is not allow to withdraw or transfer more than the balance amount.
If user key in the wrong pin more than 3 times, the bank account will be locked and the user cannever be able to loggin again.

### Assumption π:
All bank accounts are retrieved from the database.
