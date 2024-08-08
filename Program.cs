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

var fileInfo = new FileInfo(config["image"]);
if (!fileInfo.Exists)
{
    Console.WriteLine($"File {fileInfo.Name} not found");
    return;
}

var buckets = new Dictionary<int, int>();
var matrix = JsonSerializer.Deserialize<Dictionary<int, int>>(File.ReadAllText("matrix.json"));
var sb = new StringBuilder();

Console.Write("Loading File...");

using Bitmap sourceMap = (Bitmap)Image.FromFile(fileInfo.FullName);

Console.WriteLine("Done");

Console.Write("Analyzing...");

//iterate over each pixel in the bitmap
for (int i = 0; i < sourceMap.Height; i++)
{
    for (int j = 0; j < sourceMap.Width; j++)
    {
        var pixel = sourceMap.GetPixel(j,i);
        
        //calculate average brightness
        var brightness = (int)Math.Round((0.299 * pixel.R) + (0.587 * pixel.G) + (0.114 * pixel.B),0);

        var dice = 1;
        //determine dice face based on brightness
        foreach (var item in matrix)
        {
            if (brightness > item.Key)
            {
                dice= item.Value;
                break;
            }
        }

        if (buckets.ContainsKey(dice)) buckets[dice]++;
        else buckets.Add(dice, 1);
        sb.Append(dice);

    }
    sb.AppendLine();
}
Console.WriteLine("Done");

Console.WriteLine("You will need this many of each dice face:");
foreach (var item in buckets.OrderBy(o=>o.Key))
{
    Console.WriteLine($"{item.Key}: {item.Value}");
}
Console.WriteLine($"Total: {sourceMap.Width*sourceMap.Height}");

File.WriteAllText(config["output"], sb.ToString());
Console.WriteLine("Done");
