using Mediat.R.Interfaces;
namespace Mediat.R.Tests.Models;
public record TestQuery(string Input) : IQuery<SimpleResponse>;
public record TestDataQuery(string Input) : IQuery<TestData>;
