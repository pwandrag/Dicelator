# Dicelator

Converts a bitmap file into a text file that can be used pack 1000's of dice to make up the image.

### Matrix
Edit ```matrix.json``` to calibrate which values you want to assign to each dice face.

#### Example matrix.json
```json
{
  "255": 1,
  "210": 2,
  "200": 3,
  "150": 4,
  "50": 5,
  "0": 6
}
```

### Usage
```dicelator.exe --input "C:\path\to\input.bmp" --output "C:\path\to\output.txt"```