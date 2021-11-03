﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorConfigurator.Objects
{
#nullable enable
    //Interfaces
    public interface IDirectoryWrapper
    {
        public bool Exists(string? path);
        public DirectoryInfo? GetParent(string path);
        public string GetCurrentDirectory();
        public string[] GetDirectories(string path);
        public string[] GetFiles(string path);
    }
    public interface IFileWrapper
    {
        public bool Exists(string? path);
        public string[] ReadAllLines(string path);
        public string ReadAllText(string path);
        public void WriteAllLines(string path, string[] contents);
        public void WriteAllText(string path, string? contents);
    }


    //Implementations
    public class DirectoryWrapper : IDirectoryWrapper
    {
        public virtual bool Exists(string? path) => Directory.Exists(path);
        public virtual DirectoryInfo? GetParent(string path) => Directory.GetParent(path);
        public virtual string GetCurrentDirectory() => Directory.GetCurrentDirectory();
        public virtual string[] GetDirectories(string path) => Directory.GetDirectories(path);
        public virtual string[] GetFiles(string path) => Directory.GetFiles(path);
    }
    public class FileWrapper : IFileWrapper
    {
        public virtual bool Exists(string? path) => File.Exists(path);
        public virtual string[] ReadAllLines(string path) => File.ReadAllLines(path);
        public virtual string ReadAllText(string path) => File.ReadAllText(path);
        public virtual void WriteAllLines(string path, string[] contents) => File.WriteAllLines(path, contents);
        public virtual void WriteAllText(string path, string? contents) => File.WriteAllText(path, contents);
    }

#nullable disable
}
