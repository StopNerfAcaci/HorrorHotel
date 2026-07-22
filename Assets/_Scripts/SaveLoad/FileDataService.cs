// using System;
// using System.Collections.Generic;
// using System.IO;
// using UnityEngine;
//
// namespace Systems.Persistence
// {
//     public class FileDataService : IDataService
//     {
//         ISerialize _serializer;
//         string _dataPath;
//         string _fileExtension;
//
//         public FileDataService(ISerialize serializer)
//         {
//             this._dataPath = Application.persistentDataPath;
//             _fileExtension = ".json";
//             this._serializer = serializer;
//         }
//         
//         string GetPathToFile(string fileName) => Path.Combine(_dataPath, string.Concat(fileName,".", _fileExtension));
//         
//         public void Save(GameData data, bool overwrite = true)
//         {
//             string fileLocation = GetPathToFile(data.Name);
//             
//             if(!overwrite && File.Exists(fileLocation))
//                 throw new IOException($"The file '{data.Name}'.{_fileExtension}' already exists.");
//             
//             File.WriteAllText(fileLocation, _serializer.Serialize(data));
//         }
//
//         public GameData Load(string name)
//         {
//         }
//
//         public void Delete(string name)
//         {
//         }
//
//         public void DeleteAll()
//         {
//         }
//
//         public IEnumerable<string> ListSaves()
//         {
//         }
//     }
// }