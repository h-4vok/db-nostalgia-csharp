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

## Examples

Coming in next publish!

## Pending ideas/development

### Behavior changes

* Allow template method injection for you to control what happens on every action called.
* Extensibility through interfaces and composition for greater customization
* Should I be targetting 4.6.1? Figure out if there is a way to easily find what is the lesser version my code can support without changing any code.

### Structure changes

* Consider moving IDataReader extensions and ParametersBuilder to their own nuget packages and repos
* Provide more data types on IDataReaderExtensions
* Add a GetBytesNullable to IDataReaderExtensions

### Quality Changes

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