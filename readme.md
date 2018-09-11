# Database

## Main Database (sql)
Will contain main data of the application

## History Database (nosql)
Will contain changes of the data on the main database, storing always the current data on each write event of the main database per table.

## Translation Database (sql)
Will contain all the translated data on different languages.
The number of supported languages is equal to the number of db since is a copy of tables from the main database. And only adding the columns that will have a translation.

## Metadata Database (nosql)

- Will contain optimized data from the main database to be able to filter, this table is a cloned db schema of the main and will change as the main changes as well.
Will optimized the records to be able to query them more quickly and then will get the ids that should look on the main db to get those rows.

- Will have data such as last time read of this row and other values for perform clean up mantainace.

## Archive Databases
It is a clone of the main database, but this will have all the old records that are not being used anymore in a long time period, deletions and more.
This is a full copy of Main, History, Translation and Metadata.

```csharp
namespace FooNamespace
{
    public class Entity
    {
        public string Name { get; set; }
    }
}
```

| Name | Another Name | Last Name |
| - | - | - |
| Value | Value 2 | Value 4 |
| Value | Value 2 | Value 4 |
| Value | Value 2 | Value 4 |
| Value | Value 2 | Value 4 |


<div>
    <b>Hello</b>
</div>
