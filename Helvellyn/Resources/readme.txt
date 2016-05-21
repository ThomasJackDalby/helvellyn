Helvellyn
===============

A simple finacial application to report and graph accounts.
In this file, square brackets denote an input required by the user.


flag is a boolean



Functions
----------------

Import
~~~~~~~~~~~

Helvellyn can import transactions from a csv file exported from online banking. It can also import tags from a csv file which are then used to catagorise the transactions.

import {type}[-transactions/-tags] {from}[-file/-directory] {source}[]

import {-f} 


List
~~~~~~~

Displays all transactions in the datastore based upon the query.

list [tag] [scope] [param1] [param2] [param3] .. [paramN]

The parameters are based upon what flags are provided. The required flags are..

list [type] [tag] [scope]

The options for the flags are..

list [-t/--transaction] [tag] [-a/--all]
list [-t/--transaction] [tag] [-m/--month] [month] [year]

list [-tag]


Sum
~~~~~~~

Sums all transactions in the datastore based upon the query

sum [tag] [scope] [param1] [param2]

list [tag] [-a/--all]
list [tag] [-m/--month] [month] [year]

Tag
~~~~~












