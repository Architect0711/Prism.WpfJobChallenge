# Prism.WpfJobChallenge
A programming excercise I did for a job application

This is the task that was given to me which I completed in 10 hours:

Write a WPF application (preferably targeting .NET Core 3.0), which allows you to load data files that contain pairs of x and y values and display them on a graph. Once a file has been loaded, the user should be able to select one of the following fitting models: linear, exponential, or power function. The user should then be able to fit the data to the selected model. The fitted coefficients should be displayed on the screen and the fitted curve should be displayed on the graph along with the loaded data points.
  
Use a well suited, practically usable format for the data files and create some with sensible test data of your choice. Please send us the test data files along with your submission. 
  
The curve fitting models have the following equations where a and b are fitting coefficients:
Linear: y = (a * x) + b
Exponential: y = a * exp (b * x)
Power function: y = a * (x ^ b)
  
You are encouraged to use the following libraries in your solution:
Math.NET Numerics for curve-fitting: https://numerics.mathdotnet.com/
OxyPlot for graph-plotting: https://oxyplot.github.io/
  
Your submission will be judged on the clarity and simplicity of your implementation, your application's ability to handle invalid input, and the unit testing of critical components. The user interface should be intuitive and functional, but aesthetics will not be considered.
