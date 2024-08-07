// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using System.Drawing;
using System.Text;
using System.Text.Json;

var a = args[0];
IConfiguration config = new ConfigurationBuilder()
            .AddCommandLine(args)
            .Build();

if (config["image"]==null || config["output"] == null)
{
    Console.WriteLine("usage: dicelator.exe --image image.bmp --output image.txt");
    return;
}

var inputImage = config["image"];
var outputFile = config["output"];

var fileInfo = new FileInfo(inputImage);
if (!fileInfo.Exists)
{
    Console.WriteLine($"File {fileInfo.Name} not found");
}

Console.WriteLine("Analyzing...");

Bitmap b = (Bitmap)Image.FromFile(fileInfo.FullName);

var buckets = new Dictionary<int, int>();
var matrix = JsonSerializer.Deserialize<Dictionary<int, int>>(File.ReadAllText("matrix.json"));


var sb = new StringBuilder();

//iterate over each pixel in the bitmap
for (int i = 0; i < b.Height; i++)
{
    for (int j = 0; j < b.Width; j++)
    {
        var pixel = b.GetPixel(j,i);
        
        var c = (int)Math.Round((0.299 * pixel.R) + (0.587 * pixel.G) + (0.114 * pixel.B),0);

        var d = 1;
        foreach (var item in matrix)
        {
            //var d = 1;
            if (c > item.Key)
            {
                d= item.Value;
                break;
            }
        }

        if (buckets.ContainsKey(d)) buckets[d]++;
        else buckets.Add(d, 1);
        sb.Append(d);


    }
    sb.AppendLine();
}

b.Dispose();
Console.WriteLine("Matrix distribution count");
foreach (var item in buckets)
{
    Console.WriteLine($"{item.Key} {item.Value}");
}

File.WriteAllText(outputFile, sb.ToString());
Console.WriteLine("Done");