---
title: Axis and Ticks - ScottPlot 5.0 Cookbook
description: Examples of common customizations for axis labels and ticks
url: /cookbook/5.0/axis-and-ticks/
date: 1/1/2023 6:56:53 PM
---

This page is part of the [ScottPlot 5.0 Cookbook](../)

<div class="alert alert-warning" role="alert">
<strong>⚠️ WARNING:</strong> This page describes <code>ScottPlot 5.0.0-beta</code>, a preview version of ScottPlot available on NuGet. This package is not recommended for production use, and the API may change in future releases. Visit the <a href='/cookbook/4.1/'>ScottPlot 4.1 Cookbook</a> for information about the current stable version of ScottPlot.
</div>


## Axis Labels

Axis labels are the text labels centered on each axis. The text inside these labels can be changed, and the style of the text can be extensively customized.

[![](axis-labels.png)](axis-labels.png)

```cs
ScottPlot.Plot myPlot = new();

myPlot.Add.Signal(Generate.Sin(51));
myPlot.Add.Signal(Generate.Cos(51));

myPlot.XAxis.Label.Text = "Horizontal Axis";
myPlot.YAxis.Label.Text = "Vertical Axis";

myPlot.SavePng("axis-labels.png");
```


## Manually Set Axis Limits

Axis Limits can be set manually in different ways.

[![](manually-set-axis-limits.png)](manually-set-axis-limits.png)

```cs
ScottPlot.Plot myPlot = new();

myPlot.Add.Signal(Generate.Sin(51));
myPlot.Add.Signal(Generate.Cos(51));

// Interact with a specific axis
myPlot.XAxis.Min = -100;
myPlot.XAxis.Max = 150;
myPlot.YAxis.Min = -5;
myPlot.YAxis.Max = 5;

// Call a helper function
myPlot.SetAxisLimits(-100, 150, -5, 5);

myPlot.SavePng("manually-set-axis-limits.png");
```


## Read Axis Limits

The current axis limits can be read in multiple ways.

[![](read-axis-limits.png)](read-axis-limits.png)

```cs
ScottPlot.Plot myPlot = new();

myPlot.Add.Signal(Generate.Sin(51));
myPlot.Add.Signal(Generate.Cos(51));

// Interact with a specific axis
double top = myPlot.YAxis.Max;
double bottom = myPlot.YAxis.Min;

// Call a helper function
AxisLimits limits = myPlot.GetAxisLimits();
double left = limits.Rect.XMin;
double center = limits.Rect.XCenter;

myPlot.SavePng("read-axis-limits.png");
```


## Zoom to Fit Data

The axis limits can be automatically adjusted to fit the data. Optional arguments allow users to define the amount of whitespace around the edges of the data.

[![](zoom-to-fit-data.png)](zoom-to-fit-data.png)

```cs
ScottPlot.Plot myPlot = new();

myPlot.Add.Signal(Generate.Sin(51));
myPlot.Add.Signal(Generate.Cos(51));

// set limits that do not fit the data
myPlot.SetAxisLimits(-100, 150, -5, 5);

// reset limits to fit the data
myPlot.AutoScale();

myPlot.SavePng("zoom-to-fit-data.png");
```


## Frameless Plot

How to create a plot containig only the data area and no axes.

[![](frameless-plot.png)](frameless-plot.png)

```cs
ScottPlot.Plot myPlot = new();

myPlot.FigureBackground = Colors.Magenta; // should not be seen
myPlot.DataBackground = Colors.WhiteSmoke;

myPlot.Add.Signal(Generate.Sin(51));
myPlot.Add.Signal(Generate.Cos(51));

myPlot.XAxes.ForEach(x => x.IsVisible = false);
myPlot.YAxes.ForEach(x => x.IsVisible = false);
myPlot.Title.IsVisible = false;

myPlot.SavePng("frameless-plot.png");
```
