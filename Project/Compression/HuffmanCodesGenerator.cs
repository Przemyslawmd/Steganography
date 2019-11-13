
using System.Collections.Generic;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo( "Tests" )]

namespace Steganography.Huffman
{
    class HuffmanCodesGenerator
    {
        public Dictionary< byte, List< bool >> CreateCodesDictionary( NodeCompress root )
        {
            GenerateCodes( root, true );
            return codesDictionary;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void GenerateCodes( Node node, bool token )
        {
            code.Add( token );

            if ( node.Left != null )
            {
                GenerateCodes( node.Left, false );
            }
            if ( node.Right != null )
            {
                GenerateCodes( node.Right, true );
            }
            else
            {
                codesDictionary.Add( node.ByteValue, new List< bool >( code ) );
            }

            code.RemoveAt( code.Count - 1 );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private readonly List< bool > code = new List< bool >();
        private readonly Dictionary< byte, List< bool >> codesDictionary = new Dictionary< byte, List< bool >>();
    }
}

