# DBNostalgia Library for C#

Today practically everyone is working with ORMs. They come with a strong punch and immediate fast development if you know how to use them, but we usually have the issues with them.

## Understanding what DBNostalgia aims for

These are possible issues you face with ORMs today:

* SELECT statements tend to be non-ideal or under-perform
* LinQ statements (or any similar functional programming-similar practices on other languages) are a two edged sword, usually ending in bad performance for the most inexperienced engineers.
* And of course, being clouded with ORM options from the get-go, many young software engineers are not faced with the challenge of learning a sql language for the database they are using underneath.
* If you want to tune performance to the extreme, you will have a hard time doing that with an ORM. Each ORM has different limitations though.
* If you need to run truly dynamic queries, usually ORMs (tied to your models in classes) will not provide this capability.
* Usually ORMs are not tied to stored procedures, and if you want to make stored procedures work with them (say you do have support) it is usually a bit of a hassle to configure.
* Why stored procedures? There are many things to consider as differences between stored procedures and queries. You can check that in a different section from this readme.

Don't get me wrong. ORMs are great in many ways and by no means you should abandon the idea if you are aligned to my way of doing things. ORMs can work very well, especially in fresh green pasture projects where you know what you need and what you want. Also, you can be completely independent of the database technology choice, and let the ORM handle the differences in sql scripting languages.

You must also consider that usually working against an ORM feels more like a framework than a library, as you will tie your design to your ORM choice. I am not a fan of EntityFramework for example, but I have to agree the name is exactly what it is. It is a framework, and you are marrying it. I can't imagine simple bridge or proxy patterns that would allow you to easily decouple your application from EntityFramework's implementation.

DBNostalgia intends to bring a different approach. It is not an ORM, it is not a framework, it is not model-based (but it works with models), and is stored procedure centered.

## Pending ideas/development

* It forces transactions on everything. I want to change this so the developer has the power to decide that. Eventually the usage will be myUnitOfWork.Transaction or you just create a TransactionUnitOfWork.
* Allow template method injection for you to control what happens on every action called.
* Provide more data types on IDataReaderExtensions
* Extensibility through interfaces and composition for greater customization
* Add a GetBytesNullable to IDataReaderExtensions
* Consider moving IDataReader extensions and ParametersBuilder to their own nuget packages and repos
* Add GetOne and GetOneDirect methods to IUnitOfWork
* Should the Direct vs "NonDirect" (bad names) behaviors instead become different implementations/classes under the same interface? Making sure the user (developer) receives an easy-to-use library that is clear on what is its intent. This change might prove a bit difficult because of the interface we want to provide.
* Should I be targetting 4.6.1? Figure out if there is a way to easily find what is the lesser version my code can support without changing any code.
* Additional Unit testing (classes EnumerableExtensions, IDataReaderExtensions, UnitOfWork)

## Pending readme sections

* Advantages
* Disadvantages
* Examples

## Changelog 2018-09-11

### Internal changes
* Moved project to GIT
* Made necessary refactoring to reduce code
* Migrated IDataReaderExtensions and ParametersBuilder to this project.
* Added XML comments to all public interfaces.