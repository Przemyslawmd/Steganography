using System;
using System.Collections.Generic;
using System.IO;
using Compression;
using System.Windows.Forms;
using Stegan;

namespace Debug
{
    /*
      Static methods of this class can be invoked for debugging 
    */

    class DEBUG
    {
        public static void WriteNodeList(List<NodeCompress> Nodes, String Title)
        {
            FileStream File = new FileStream("Debug.txt", FileMode.Append);
            StreamWriter Stream = new StreamWriter(File);

            Stream.WriteLine();
            Stream.WriteLine("***************************************************************************************************");
            Stream.WriteLine("***************************************************************************************************");
            Stream.WriteLine(" {0}\n", Title);

            foreach ( NodeCompress node in Nodes )
                Stream.WriteLine( " Value: {0,2:X}; Count: {1, 4}", node.ByteValue, node.Count.ToString() );

            Stream.WriteLine();
            Stream.Close();
            File.Close();   
        }

        /***************************************************************************************************************************/
        /***************************************************************************************************************************/

        public static void WriteHuffmanTreeCompression( List<NodeCompress> Tree, String Title )
        {
            FileStream File = new FileStream( "Debug.txt", FileMode.Append );
            StreamWriter Stream = new StreamWriter( File );

            Stream.WriteLine();
            Stream.WriteLine("***************************************************************************************************");
            Stream.WriteLine("***************************************************************************************************");
            Stream.WriteLine(" {0}\n", Title);

            InOrderTreeCompression( Tree[0], Stream );
            Stream.Close();
            File.Close();   
        }

        /*************************************************************************************************************************/
        /*************************************************************************************************************************/

        public static void WriteHuffmanTreeDecompression( NodeDecompress root, String Title )
        {
            FileStream File = new FileStream( "Debug.txt", FileMode.Append );
            StreamWriter Stream = new StreamWriter( File );

            Stream.WriteLine();
            Stream.WriteLine("***************************************************************************************************");
            Stream.WriteLine("***************************************************************************************************");
            Stream.WriteLine(" {0}\n", Title);

            InOrderTreeDecompression( root, Stream );
            Stream.Close();
            File.Close();
        }
        
        /****************************************************************************************************************************/
        /****************************************************************************************************************************/
        
        public static void WriteData( Byte[] Data, String Title )
        {
            FileStream file = new FileStream( "Debug.txt", FileMode.Append );
            StreamWriter Stream = new StreamWriter( file );

            Stream.WriteLine();
            Stream.WriteLine("***************************************************************************************************");
            Stream.WriteLine("***************************************************************************************************");
            Stream.WriteLine(" {0}\n", Title);

            for ( int i = 0; i < Data.Length; i++ )
            {
                if ( i % 20 == 0 ) Stream.WriteLine();
                Stream.Write("{0,2:X} : ", Data[i]);                
            }

            Stream.WriteLine();
            Stream.Close();
            file.Close();
        } 
   
        /**************************************************************************************************************************/
        /**************************************************************************************************************************/

        public static void WriteCodes( Dictionary<Byte, String> Codes, String Title )
        {
            FileStream file = new FileStream( "Debug.txt", FileMode.Append );
            StreamWriter Stream = new StreamWriter( file );
            
            Stream.WriteLine();
            Stream.WriteLine("***************************************************************************************************");
            Stream.WriteLine("***************************************************************************************************");
            Stream.WriteLine(" {0}\n", Title);

            foreach ( KeyValuePair<Byte, string> Item in Codes )            
                Stream.WriteLine( "Value: {0,4:X}; Code: {1,6}", Item.Key, Item.Value );         

            Stream.WriteLine();
            Stream.Close();
            file.Close();
        }

        /****************************************************************************************************************************/
        /****************************************************************************************************************************/

        public static void WriteCodes2( List <Byte> Bytes, List<Char[]> Codes, String Title )
        {
            FileStream file = new FileStream( "Debug.txt", FileMode.Append );
            StreamWriter Stream = new StreamWriter( file );

            Stream.WriteLine();
            Stream.WriteLine("***************************************************************************************************");
            Stream.WriteLine("***************************************************************************************************");
            Stream.WriteLine(" {0}\n", Title);
            
            for ( int i = 0; i < Bytes.Count; i++ )            
                Stream.WriteLine( " Value: {0,4:X}; Code: {1,6}", Bytes[i], new String(Codes[i]) );

            Stream.WriteLine();
            Stream.Close();
            file.Close();
        }


        /***************************************************************************************************************************/
        /***************************************************************************************************************************/

        private static void InOrderTreeCompression( NodeCompress Node, StreamWriter Stream )
        {
            if ( Node != null )
            {
                InOrderTreeCompression( Node.Left, Stream );
                Stream.WriteLine( " Value: {0,4:X}; Count: {1,4}; IsLeaf: {2,4}", Node.ByteValue, Node.Count, Node.IsLeaf );
                InOrderTreeCompression( Node.Right, Stream );
            }
        }

        /************************************************************************************************************************/
        /************************************************************************************************************************/
        
        private static void InOrderTreeDecompression( NodeDecompress Node, StreamWriter Stream )
        {
            if ( Node != null )
            {
                InOrderTreeDecompression( Node.Left, Stream );
                try
                {
                    Stream.WriteLine( " Value: {0,4:X}; IsLeaf: {1,4}", Node.ByteValue, Node.Leaf );
                }
                catch (Exception e)
                {
                    MessageBox.Show( e.Message.ToString() );                    
                }
                InOrderTreeDecompression( Node.Right, Stream );
            }
        }
    }
}
