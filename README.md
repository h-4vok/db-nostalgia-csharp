# DBNostalgia Library for C#

Today practically everyone is working with ORMs. They come with a strong punch and immediate fast development if you know how to use them, but we usually have the issues with them.

DBNostalgia brings in another alternative. Say you want to work with your own stored procedures, but usually ORMs are very difficult to combine with them. Many times you are missing capabilities like execution plan caching and having an easier time performance tuning your database. Maybe your DBAs hate your application because of the crazy and unnecesary complex queries your ORM creates. Maybe in your company you just can't have a project without stored procedures.

Using ADO.NET can be very cumbersome and there is a LOT to care for which should be unnecesary. While there are many ways to work with ADO.NET, DBNostalgia provides an opinionated open source idea on how to work against your database using your very own stored procedures. Want to use models? Not a problem. Do you just want to read a single value? Fine! You just want to run a god d***** procedure, then just do it.

I will be expanding the Examples section while I continue shipping new features and improvements. It is my first NuGet package so expect some breaking changes here and there. I will be documenting everything here.

## Understanding what DBNostalgia aims for

These are possible issues you face with ORMs today:

* SELECT statements tend to be non-ideal or under-perform
* LinQ statements (or any similar functional programming-similar practices on other languages) are a two edged sword, usually ending in bad performance for the most inexperienced engineers.
* And of course, being clouded with ORM options from the get-go, many young software engineers are not faced with the challenge of learning a sql language for the database they are using underneath.
* If you want to tune performance to the extreme, you will have a hard time doing that with an ORM. Each ORM has different limitations though.
* If you need to run truly dynamic queries, usually ORMs (tied to your models in classes) will not provide this capability.
* Usually ORMs are not tied to stored procedures, and if you want to make stored procedures work with them (say you do have support) it is usually a bit of a hassle to configure.
* You miss out execution plan caching.
* You depend on how the ORM handles your requests.
* Many times you need to hidrate data just to change one column or something very small.
* Working with collections within collections can be extra cumbersome and many times, always ineffective or inefficient.
* Why stored procedures? There are many things to consider as differences between stored procedures and queries. You can check that in a different section from this readme.

Don't get me wrong. ORMs are great in many ways and by no means you should abandon the idea if you are aligned to my way of doing things. ORMs can work very well, especially in fresh green pasture projects where you know what you need and what you want. Also, you can be completely independent of the database technology choice, and let the ORM handle the differences in sql scripting languages.

You must also consider that usually working against an ORM feels more like a framework than a library, as you will tie your design to your ORM choice. I am not a fan of EntityFramework for example, but I have to agree the name is exactly what it is. It is a framework, and you are marrying it. I can't imagine simple bridge or proxy patterns that would allow you to easily decouple your application from EntityFramework's implementation.

DBNostalgia intends to bring a different approach. It is not an ORM, it is not a framework, it is not model-based (but it works with models), and is stored procedure centered.

## Getting Started

### Prerequisites

* Currently working for .NET 4.6.1. I will be lowering this target gradually.
* No dependencies yet, but there is functionality here which I will move to independent NuGet packages.

## Connecting to the database and starting to work

It is pretty simple. Start by creating your unit of work object.

```
Func<IDbConnection> createDbClosure = () => new SqlConnection(aConnectionString);

var uow = new UnitOfWork(createDbClosure);

uow.NonQueryDirect("someSp");
```

Let's explain each part. UnitOfWork is expecting a closure that will produce a IDbConnection object. This means you need to provide how the IDbConnection is going to be built. This way, you have 100% control of how connections are used. What UnitOfWork will do with this is calling your procedure when it needs to open a connection.

Wish to always re-use the same connection? Maybe you have a bunch of database connections? Just send the closure that UnitOfWork needs to be able to point to your connection when the time comes.

After we created our object, uow.NonQueryDirect did the following:
* Used your closure to create a connection
* Connected to your database
* Ran the stored procedure "someSp" without any parameters.

This is the most simple scenario.

You can use UnitOfWork wherever you want. I generally recommend to have one unit of work per repository class, if you prefer to use that pattern.

If you wish to know what else you can do, take a look at the Examples section!

### Small note

UnitOfWork will have no problem working in scenarios like Web applications, but keep in mind you are telling UnitOfWork how to get the IDbConnection to use. Consider things like multi-threading and such just like you would in any other library.

Again, just remember that UnitOfWork uses a connection object, so using the same UnitOfWork in a multi-threaded scenario is not recommended. That is a pending feature, but UnitOfWork is not memory heavy nor using the same connection in multi-threading is within DBNostalgia scope right now. However I can envision that just being another class that inherits from IUnitOfWork but has a different behavior and management.

## Examples

For every example we will assume there is a this.UnitOfWork object alive. You can see how to create your unit of work from the Getting Started section.

Keep in mind that the ParametersBuilder class is it's own thing, and will eventually be moved to it's own nuget package. ParametersBuilder is a mix between a Builder and a Visitor pattern. It will allow you to quickly setup parameters for IDbCommand objects, and then visit them to hidrate the Parameters properly when needed.

### NonQueryDirect

NonQueryDirect is a method that will allow you to run stored procedures that produce no result.
Or perhaps they do, but you do not care.

First, let's simply run a stored procedure called "execution_noParams". As you can guess, this one has no parameters.

```
this.UnitOfWork.NonQueryDirect("execution_noParams");
```

By using NonQueryDirect, the connection was automatically opened without transaction. You will see how to run within a transaction later on in the Run() section.

Now let's run another stored procedure called "MyModel_delete" which receives an ID parameter.

```
var id = 100;
this.UnitOfWork.NonQueryDirect("MyModel_delete", ParametersBuilder.With("id", id));
```

Here we called "MyModel_delete" with parameter @id = 100.

### ScalarDirect

ScalarDirect is a method that will allow you to run a stored procedure and do what usually ExecuteScalar does. Read the value of the first row and first column (that is actually the real implementation). In other words, get a single value.

Let's run a stored procedure "scalar_noParams"

```
var result = this.UnitOfWork.ScalarDirect("scalar_noParams");
```

Here we have opened the connection, executed "scalar_noParams" and obtained the result. That result is on the "result" variable which is of type object. If you wish to read this as a particular type, you need to cast or convert as you prefer.

If you wish to use parameters, like in every example, we simply use ParametersBuilder.

```
var result = this.UnitOfWork.ScalarDirect("scalar_withParams",
    ParametersBuilder.With("stringParam", "string value")
    .And("intParam", 1000)
    .And("bitParam", true)
);
```

There is a library of my own I use called ObjectExtensions, which is not yet in NuGet. I will first add it to DBNostalgia and then move it to NuGet as a separate dependency eventually. These allow you to perform very fast transformations, for example we can change the above example to read like an integer.

```
var integerResult = this.UnitOfWork.ScalarDirect("scalar_noParams").AsInt();
```

That will stay optional anyway, so do not worry if you do not like this approach.

### GetOneDirect

GetOneDirect is a method that will run a stored procedure, open an IDataReader internally and allow you to read all the data from that first row. You can turn the data into a model, or just do whatever you want with it.

Let's run a stored procedure "MyModel_get" with an ID parameter. We will create an object of class MyModel during this execution.

```
var id = 22;

var model = this.UnitOfWork.GetOneDirect(
    "MyModel_get",
    (reader) => {
        return new MyModel 
        {
            Property1 = reader.GetString("Property1"),
            Property2 = reader.GetInt32("Property2"),
            Property3 = reader.GetBool
        }
    },
    ParametersBuilder.With("id", id)
);
```

## Pending ideas/features

### Behavior changes

* Allow template method injection for you to control what happens on every action called.
* Extensibility through interfaces and composition for greater customization
* Should I be targetting 4.6.1? Figure out if there is a way to easily find what is the lesser version my code can support without changing any code.

### Structure changes

* Consider moving IDataReader extensions and ParametersBuilder to their own nuget packages and repos
* Provide more data types on IDataReaderExtensions
* Add a GetBytesNullable to IDataReaderExtensions

### Quality Changes

* Usage of ObjectExtensions for faster conversions.
* Move ObjectExtensions to its own nuget package.
* Additional Unit testing (classes EnumerableExtensions, IDataReaderExtensions, UnitOfWork)
* Should the Direct vs "NonDirect" (bad names) behaviors instead become different implementations/classes under the same interface? Making sure the user (developer) receives an easy-to-use library that is clear on what is its intent. This change might prove a bit difficult because of the interface we want to provide.

## Pending sections

* Advantages
* Disadvantages

# Changelogs

## Changelog 2018-09-18
* Such a bad luck! There was a nasty bug that rendered the whole library unusable. This happened because I kept pushing unit testing for another day. Bug is fixed.
* Added integration testing for the UnitOfWork class. I will be adding more testing the more I develop. These tests prove enough operations.
* 

## Changelog 2018-09-17
* Added GetOne and GetOneDirect methods.
* Added NuGet configuration and icon
* Finally published to NuGet.org! Now it's time to make this worth the download.

## Changelog 2018-09-11

### Internal changes
* Moved project to GIT
* Made necessary refactoring to reduce code
* Migrated IDataReaderExtensions and ParametersBuilder to this project.
* Added XML comments to all public interfaces.