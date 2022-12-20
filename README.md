# Arcoreimg GUI

[arcoreimg](https://developers.google.com/ar/develop/c/augmented-images/arcoreimg) is a command line tool that takes a set of reference images and generates an image database file. 

Arcoreimg GUI is built upon this tool and is a nice alternative if you don't want to use command line.

![arcoreimg Screenshot](images/ARCoreImg.png "arcoreimg Screenshot")

## Features

1. Check image quality
2. Create an image database file from a directory of images
3. Create an image database file from an image list file


## Differences with JacksiroKe's version

[The original version of this application was made by JacksiroKe](https://github.com/JacksiroKe/arcoreimg). It is not maintained anymore and had some issues.

This version has a few fixes, some improvement and fewer dependencies.

A brief and incomplete summary of the differences :
- Fixed an issue that would prevent evaluating images that have spaces in their path. 
- Fixed most encountered bugs for the image evaluation.
- Evaluating multiple images at once is faster.
- Added the possibility to select multiple images instead of one for the image evaluation.
- Added the possibility to drag and drop images for the image evaluation.
- Removed Dragablz, MahApps, ControlzEx, MaterialDesign, Windows.Interactivity dependencies.
- Because MaterialDesign was removed, the UI is less pretty, but still usable.

You can also check [the complete changelog here](CHANGELOG.md).
