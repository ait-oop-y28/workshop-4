// See https://aka.ms/new-console-template for more information

using Workshop4.Application.Extensions;
using Workshop4.Application.Pipelines.Nodes;

var filter = new EnabledNode(new FilterNode()) { IsEnabled = false };
var map = new MappingNode();

var group = new GroupNode();
group.AddNode(filter);
group.AddNode(map);

var nodes = group.Enumerate().ToArray();

Console.WriteLine(nodes);
