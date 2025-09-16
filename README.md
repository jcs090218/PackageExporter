[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)
[![Unity Engine](https://img.shields.io/badge/unity-6000.2.4f1-black.svg?style=flat&logo=unity)](https://unity3d.com/get-unity/download/archive)
[![.NET](https://img.shields.io/badge/.NET-2.0-blueviolet.svg)](https://docs.unity3d.com/2018.3/Documentation/Manual/ScriptingRuntimeUpgrade.html)
[![Release](https://img.shields.io/github/tag/jcs090218/PackageExporter.svg?label=release&logo=github)](https://github.com/jcs090218/PackageExporter/releases/latest)
[![openupm](https://img.shields.io/npm/v/com.jcs090218.package-exporter?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.jcs090218.package-exporter/)
[![openupm](https://img.shields.io/badge/dynamic/json?color=brightgreen&label=downloads&query=%24.downloads&suffix=%2Fmonth&url=https%3A%2F%2Fpackage.openupm.com%2Fdownloads%2Fpoint%2Flast-month%2Fcom.jcs090218.package-exporter)](https://openupm.com/packages/com.jcs090218.package-exporter/)

# Package Exporter

[![License](https://github.com/jcs090218/PackageExporter/actions/workflows/license.yml/badge.svg)](https://github.com/jcs090218/PackageExporter/actions/workflows/license.yml)
[![Source ⚙️](https://github.com/jcs090218/PackageExporter/actions/workflows/source_build.yml/badge.svg)](https://github.com/jcs090218/PackageExporter/actions/workflows/source_build.yml)
[![Source 📦](https://github.com/jcs090218/PackageExporter/actions/workflows/source_package.yml/badge.svg)](https://github.com/jcs090218/PackageExporter/actions/workflows/source_package.yml)
[![Publish ⚙️](https://github.com/jcs090218/PackageExporter/actions/workflows/publish_build.yml/badge.svg)](https://github.com/jcs090218/PackageExporter/actions/workflows/publish_build.yml)
[![Publish 📦](https://github.com/jcs090218/PackageExporter/actions/workflows/publish_package.yml/badge.svg)](https://github.com/jcs090218/PackageExporter/actions/workflows/publish_package.yml)
[![Docs](https://github.com/jcs090218/PackageExporter/actions/workflows/docs.yml/badge.svg)](https://github.com/jcs090218/PackageExporter/actions/workflows/docs.yml)

Export multiple packages by just one click, with no need to uncheck the assets.

This package is a packaging (exporting) tool for Unity package system. It
allows you to export multiple packages with different contents.

*P.S. Notice this package will only work in Editor, and does not expect user
to use it in production builds.*

## 📁 Project Structures

* `Source` - Project contains plugin's source code.
* `Compile` - Project compiles source to DLL, it will link `_references` DLLs.
* `_references` - Unity DLL to compile project source to DLL.
* `Publish` - Project that contains package DLL and ready to publish to [Asset Store Publisher](https://publisher.assetstore.unity3d.com/info.html?_gl=1*1fwg1ij*_ga*MTg0NjU4MTc4NC4xNjAwMzQ5NzM3*_ga_1S78EFL1W5*MTYyNDI3MzU4Ni40Ni4wLjE2MjQyNzM1ODYuNjA.&_ga=2.77544981.1416380940.1624186429-1846581784.1600349737) portal.

## 🔗 Links

* [Documentation](https://jcs090218.github.io/PackageExporter/)

## License

Copyright (c) 2017-2025 Jen-Chieh Shen

Licensed under MIT. See [LICENSE.txt](https://github.com/jcs090218/PackageExporter/blob/master/LICENSE.txt) for details.
