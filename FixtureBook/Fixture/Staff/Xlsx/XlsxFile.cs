/*
 * Copyright 2013 XPFriend Community.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using XPFriend.Junk;

namespace XPFriend.Fixture.Staff.Xlsx
{
    /// <summary>
    /// .xlsx ファイル。
    /// </summary>
    internal class XlsxFile
    {
        private string fileName;
        private ZipFile zipFile;
	    private XlsxCellParser xlsxCellParser;
	    private XlsxWorkbook workbook;
	    private XlsxStyles styles;
	    private XlsxSharedStrings sharedStrings;
        
        /// <summary>
        /// <see cref="XlsxFile"/> を作成する。
        /// </summary>
        /// <param name="fileName">読み込むファイルのパス</param>
        /// <param name="xlsxCellParser">ワークシート処理</param>
	    public XlsxFile(string fileName, XlsxCellParser xlsxCellParser)
        {
            this.fileName = fileName;
		    this.xlsxCellParser = xlsxCellParser;
		    ReadWorkbookInformation();
	    }

        /// <summary>
        /// ワークシートの一覧。
        /// </summary>
        public List<string> WorksheetNames { get { return workbook.SheetNames; } }

	    private Stream GetInputStream(String name) 
        {
		    ZipEntry entry = zipFile[name];
		    if (entry != null) 
            {
                return entry.OpenReader();
		    } 
            else 
            {
			    return null;
		    }
	    }

	    private void ReadWorkbookInformation()
        {
		    try 
            {
                this.zipFile = ZipFile.Read(fileName);
                using (Stream stream = GetInputStream("xl/workbook.xml")) { workbook = new XlsxWorkbook(stream); }
                using (Stream stream = GetInputStream("xl/styles.xml")) { styles = new XlsxStyles(stream); }
                using (Stream stream = GetInputStream("xl/sharedStrings.xml")) { sharedStrings = new XlsxSharedStrings(stream); }
		    } 
            finally
            {
			    Close();
		    }
	    }

        /// <summary>
        /// 指定された名前のワークシートを読み込む。
        /// </summary>
        /// <param name="sheetName">ワークシート名</param>
        public void Read(String sheetName)
        {
            try
            {
                XlsxWorksheetHandler handler = CreateHandler();
                String fileName = workbook.GetFileName(sheetName);
                if (fileName == null)
                {
                    throw new ConfigException("M_Fixture_XlsxFile_Read", sheetName);
                }
                using (Stream stream = GetInputStream(fileName))
                {
                    Parse(handler, stream);
                }
            }
            finally
            {
                Close();
            }
        }

        private XlsxWorksheetHandler CreateHandler()
        {
            zipFile = ZipFile.Read(fileName);
            return new XlsxWorksheetHandler(styles, sharedStrings, xlsxCellParser);
        }

        private void Parse(XlsxWorksheetHandler handler, Stream sheet)
        {
		    handler.Parse(sheet);
	    }

        private void Close()
        {
            try
            {
                if (zipFile != null)
                {
                    zipFile.Dispose();
                }
            }
            finally
            {
                zipFile = null;
            }
        }
    }
}
