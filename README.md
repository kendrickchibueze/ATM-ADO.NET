# Welcome to the Talent Bank ATM Machine with ADO.NET✌🤞👌😉😎👓👓
This is  an ATM console application built with C#👓.It is designed using object oriented programming(OOP) construct and SQL database. An explicit look at our Entity relationship diagram for my database looks like this:

 ![](https://github.com/kendrickchibueze/-Modern-Node-on-AWS/blob/main/aws-images/Screenshot%20(499).png?raw=true)



A pragmatic run of ourexecutable assembly looks like this:

![](https://raw.githubusercontent.com/kendrickchibueze/-Modern-Node-on-AWS/5d6752d563ac41bcdf4c1419a5337a4dcae2cbf4/aws-images/Screenshot%20(395).png)

## Software Development Summary😃👓👓
* **Technology**: C#👓
* **Framework**: .NET6
* **Project Type**: Console
* **IDE**: Visual Studio (Version 2022)
* **Paradigm or pattern of programming**: Object-Oriented Programming (OOP)
* **Data**: Data of this demo program (Bank Account and Transaction data) are stored using List objects. No database is used on purpose for this demo version.

 ## ATM Basic Features / Use Cases 👓👌👌:
 * Check account balance
 * Place deposit
 * Make withdraw
 * Check card number and pin against bank account list object (Note: An SQL database is used for this project)
 * View All transactions
 * Make transfer (Transfer within the same bank but different account number)
 ## Logic
* **Business Rules** 🤷‍♀️:
User is not allow to withdraw or transfer more than the balance amount.
If user key in the wrong pin more than 3 times, the bank account will be locked and the user cannever be able to loggin again.

### Assumption 🎗:
All bank accounts are retrieved from the database.
