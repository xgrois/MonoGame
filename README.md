# MonoGame
MonoGame simple projects

## Contents
[[1] **ConsoleAppWithMonogame.**](https://github.com/xgrois/MonoGame/blob/master/ConsoleAppWithMonoGame)  This is a .NET 5 console app (not using a MonoGame template) that adds MonoGame as a nuget package.
MonoGame does not provide shapes so in this sample we create some helpful classes to draw simple "line shapes" (line, circle, convex regular polygons) easier and
without the need of the content manager tool.


![Image 1](https://github.com/xgrois/MonoGame/blob/master/ConsoleAppWithMonoGame/Capture.JPG)


[[2] **MonoGameWithPhysics.**](https://github.com/xgrois/MonoGame/blob/master/MonoGameWithPhysics) This is a MonoGame (OpenGL template) project (changed from WindowsApp to ConsoleApp and from Core 3.1 to .NET 5 in project properties) that adds realistic physics usign a pure C# port of the well-known Box2D library. This port is [Aether2D](https://github.com/tainicom/Aether.Physics2D) and adds extra stuff. In this project you can create new balls by clicking the left-mouse and move the orange ball via keyboard inputs, gamepad inputs and also apply some force to the ball using the right-mouse click (similar to "angry-birds" game).

![Image 2](https://github.com/xgrois/MonoGame/blob/master/MonoGameWithPhysics/Capture.JPG)
