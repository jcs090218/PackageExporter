## Package upload

### 📝 Additional compatibility information

Any version above 5.6+ should work. Please see Technical detail section for
more information.

## Description

### 📝 Summary (10-200 characters)

**Package Exporter** is a packaging (exporting) tool for Unity package system.
It allows you to export multiple packages with different contents.

### 📝 Description

This is often used when you have multiple target to export, but it's hard to
memorize all contents to export for each package. This tool resolve these
issues, and provides a method similar to Git (Source Control) ignore system.

#### Features:

- Export multiple packages in a project
- Export method similar to `.gitignore` (easy for coder)
- Lightweight, no other dependencies
- Clean, no files are generated
- Highly compatible to any Unity version

*P.S. Notice this package will only work in Editor, and does not expect user
to use it in production builds.*

If you have any issue please contact us through [GitHub](https://github.com/jcs090218)
or email to jcs090218@gmail.com.

[More Info>>](https://github.com/jcs090218/PackageExporter)

### 📝 Technical details

The package uses Unity Packaging API and since Unity has established the
packaging system in very early version, it will work on most of the Unity
version. Thus, I will recommend you work use this package wit minimum
version 5.6 or above.
