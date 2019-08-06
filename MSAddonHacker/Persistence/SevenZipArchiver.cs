using System;
using System.Collections.Generic;
using System.IO;
using SevenZip;

namespace MSAddonHacker.Persistence
{
    public class SevenZipArchiver
    {
        /// <summary>
        /// Texto del último error ocurrido en alguna operación
        /// </summary>
        public string LastErrorText { get; private set; }

        public string ArchiveName { get; private set; }


        public SevenZipArchiver(string pArchiveName)
        {
            ArchiveName = pArchiveName?.Trim();
        }


        // ----------------------------------------------------------------------------------------

        /// <summary>
        /// Retorna el contenido de un archivo
        /// </summary>
        /// <param name="pEntryList">Lista de entradas</param>
        /// <returns>Número de entradas en el archivo (-1=error)</returns>
        public int ArchivedFileList(out List<ArchiveFileInfo> pEntryList)
        {
            pEntryList = null;
            try
            {
                if (string.IsNullOrEmpty(ArchiveName) || !File.Exists(ArchiveName))
                {
                    LastErrorText = "Invalid archive file name specification/file not found";
                    return -1;
                }

                using (SevenZipExtractor extractor = new SevenZipExtractor(ArchiveName))
                {
                    pEntryList = new List<ArchiveFileInfo>();
                    foreach (ArchiveFileInfo item in extractor.ArchiveFileData)
                    {
                        pEntryList.Add(item);
                    }
                }
            }
            catch (Exception exception)
            {
                LastErrorText = $"EXCEPTION: {exception.Message}";
                pEntryList = null;
                return -1;
            }

            return pEntryList.Count;
        }


        // -----------------------------------------------------------------------------------------------------

        /// <summary>
        /// Extrae ficheros de un archivo  
        /// </summary>
        /// <param name="pDestinationPath">directorio de destino de los ficheros extraídos</param>
        /// <param name="pFileList">Lista con especificaciones de ficheros a extraer. Admite máscara de ficheros</param>
        /// <remarks>Actualmente, no funciona el filtro por lista de nombre de ficheros</remarks>
        /// <returns>Número de ficheros extraídos. -1 si error</returns>
        public int ArchivedFilesExtract(string pDestinationPath, List<string> pFileList)
        {
            int fileExtractedCount = 0;
            try
            {
                if (string.IsNullOrEmpty(ArchiveName) || !File.Exists(ArchiveName))
                {
                    LastErrorText = "Invalid archive file name specification/file not found";
                    return -1;
                }

                using (SevenZipExtractor extractor = new SevenZipExtractor(ArchiveName))
                {
                    if ((pFileList?.Count ?? -1) <= 0)
                    {
                        extractor.ExtractArchive(pDestinationPath);
                    }
                    else
                    {
                        List<string> lwrFileList = new List<string>();
                        foreach(string item in pFileList)
                            lwrFileList.Add(item.ToLower().Trim());
                        List<int> fileIndexes = new List<int>();
                        for (int index = 0; index < extractor.ArchiveFileData.Count; ++index)
                        {
                            string filename = extractor.ArchiveFileData[index].FileName;
                            if (lwrFileList.Contains(filename.ToLower().Trim()))
                                fileIndexes.Add(index);
                        }
                        fileExtractedCount = fileIndexes.Count;
                        if (fileExtractedCount > 0)
                        {
                            extractor.ExtractFiles(pDestinationPath, fileIndexes.ToArray());
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LastErrorText = $"EXCEPTION: {exception.Message}";
                return -1;
            }

            return fileExtractedCount;

        }



        public byte[] ExtractArchivedFileToByte(string pFilename)
        {
            byte[] buffer = null;
            try
            {
                if (string.IsNullOrEmpty(ArchiveName) || !File.Exists(ArchiveName))
                {
                    LastErrorText = "Invalid archive file name specification/file not found";
                    return null;
                }
                if (string.IsNullOrEmpty(pFilename = pFilename?.Trim().ToLower()))
                    return null;


                using (SevenZipExtractor extractor = new SevenZipExtractor(ArchiveName))
                {
                    int fileIndex = -1;
                    for (int index = 0; index < extractor.ArchiveFileData.Count; ++index)
                    {
                        string filename = extractor.ArchiveFileData[index].FileName.ToLower();
                        if (filename == pFilename)
                        {
                            buffer = new byte[extractor.ArchiveFileData[index].Size];
                            fileIndex = index;
                            break;
                        }
                    }
                    if (fileIndex > -1)
                    {

                        using (MemoryStream stream = new MemoryStream(buffer))
                        {
                            extractor.ExtractFile(fileIndex, stream);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LastErrorText = $"EXCEPTION: {exception.Message}";
                return null;
            }

            return buffer;
        }

        // ----------------------------------------------------------------------------------------------------------------------------------------------------


        public bool ArchiveFolder(string pFolderToCompress, string pPassword = null, 
            bool pEncryptHeaders = false,
            OutArchiveFormat pArchiveFormat = OutArchiveFormat.Zip, 
            CompressionLevel pCompressionLevel = CompressionLevel.Normal,
            CompressionMethod pCompressionMethod = CompressionMethod.Default
         )
        {

            if (string.IsNullOrEmpty(ArchiveName))
            {
                LastErrorText = "CreateArchive(): Invalid archive specification";
                return false;
            }

            if (string.IsNullOrEmpty(pFolderToCompress = pFolderToCompress?.Trim()) || !Directory.Exists(pFolderToCompress))
            {
                LastErrorText = "CreateArchive(): Invalid folder specification or folder not found";
                return false;
            }

            bool archiveOk = false;
            string backupFile = null;
            try
            {
                if (File.Exists(ArchiveName))
                {
                    backupFile = ArchiveName + ".bak";
                    File.Move(ArchiveName, backupFile);
                }

                SevenZipCompressor archiver = new SevenZipCompressor();
                archiver.CompressionMode = CompressionMode.Create;
                archiver.ArchiveFormat = pArchiveFormat;
                archiver.CompressionLevel = pCompressionLevel;
                archiver.CompressionMethod = pCompressionMethod;

                if (!string.IsNullOrEmpty(pPassword = pPassword?.Trim()))
                {
                    archiver.EncryptHeaders = pEncryptHeaders;
                    archiver.CompressDirectory(pFolderToCompress, ArchiveName, true, pPassword);
                }
                else
                    archiver.CompressDirectory(pFolderToCompress, ArchiveName);
                
                archiveOk = File.Exists(ArchiveName);
            }
            catch (Exception exception)
            {
                LastErrorText = $"CreateArchive(): {exception.Message}";
            }
            finally
            {
                if (backupFile != null)
                {
                    if(archiveOk)
                        File.Delete(backupFile);
                    else
                        File.Move(backupFile, ArchiveName);
                }
            }
            return archiveOk;
        }

    }
}
