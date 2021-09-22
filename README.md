**This page only describes the project structure, please visit the source folder to see
the full documentation.**

[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)
[![Unity Engine](https://img.shields.io/badge/unity-2021.1.1f1-black.svg?style=flat&logo=unity&cacheSeconds=2592000)](https://unity3d.com/get-unity/download/archive)

---

#### 📝 Source

[![Build Source](https://github.com/Pixisoft/PackageExporter/actions/workflows/build_source.yml/badge.svg)](https://github.com/Pixisoft/PackageExporter/actions/workflows/build_source.yml)
[![Package Source](https://github.com/Pixisoft/PackageExporter/actions/workflows/package_source.yml/badge.svg)](https://github.com/Pixisoft/PackageExporter/actions/workflows/package_source.yml)

* `Source` - Project contains plugin's source code.

#### 💬 Compiling

* `Compile` - Project compiles source to DLL, it will link `_references` DLLs.
* `_references` - Unity DLL to compile project source to DLL.

#### ✒️ Publishing

[![Build Publish](https://github.com/Pixisoft/PackageExporter/actions/workflows/build_publish.yml/badge.svg)](https://github.com/Pixisoft/PackageExporter/actions/workflows/build_publish.yml)
[![Package Publish](https://github.com/Pixisoft/PackageExporter/actions/workflows/package_publish.yml/badge.svg)](https://github.com/Pixisoft/PackageExporter/actions/workflows/package_publish.yml)

* `Publish` - Project that contains package DLL and ready to publish to [Asset Store Publisher](https://publisher.assetstore.unity3d.com/info.html?_gl=1*1fwg1ij*_ga*MTg0NjU4MTc4NC4xNjAwMzQ5NzM3*_ga_1S78EFL1W5*MTYyNDI3MzU4Ni40Ni4wLjE2MjQyNzM1ODYuNjA.&_ga=2.77544981.1416380940.1624186429-1846581784.1600349737) portal.
