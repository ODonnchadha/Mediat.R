using Mediat.R.Interfaces;

namespace Mediat.R.Tests.Models;
public record TestCommand(string Input) : ICommand<SimpleResponse>;
public record TestDataCommand(string Input) : ICommand<TestData>;
