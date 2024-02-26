using BattleCombine.Data;
using BattleCombine.Gameplay;
using UnityEngine;
using UnityEngine.PlayerLoop;


namespace BattleCombine.Services
{
    public class LevelDesignSaveField : MonoBehaviour
    {
        [SerializeField] private bool encryptData;
        [SerializeField] private LevelDesignCreateField fieldScript;
        
        [Header("For save and load field files")]
        [SerializeField] private string fileName;

        private LevelDesignFieldData _fieldData;
        private LevelDesignFileHandler _dataHandler;
        private string _fileFolderPath;
        private string _fileName;

        private void Initialization()
        {
            _fileFolderPath = Application.dataPath + "/Resources/LevelFiles/";
            _fileName = fileName;
            encryptData = false;
            _dataHandler = new LevelDesignFileHandler(_fileFolderPath, _fileName, encryptData);
        }

        public bool CheckForSavedData()
        {
            _fieldData = _dataHandler.Load();
            return _fieldData != null;
        }

        public void LoadFieldData()
        {
            Initialization();
            if (!CheckForSavedData())
            {
                Debug.Log("File not found");
                return;
            }
            _fieldData = _dataHandler.Load();
            fieldScript.GenerateDefaultField();
            fieldScript.LoadFieldData(_fieldData);
        }

        public void SaveFieldData()
        {
            Initialization();
            _fieldData ??= new LevelDesignFieldData();
            fieldScript.SaveFieldToFile(ref _fieldData);
            _dataHandler.Save(_fieldData);
        }
        
        [ContextMenu("Delete Save Data")]
        public void DeleteSavedData()
        {
            Initialization();
            _dataHandler.Delete();
        }
    }
}