
using System.Collections.Generic;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo( "Tests" )]

namespace SteganographyCompression
{
    class HuffmanCodesGenerator
    {
        public Dictionary< byte, List< bool >> CreateCodesDictionary( NodeCompress root )
        {
            codesDictionary = new Dictionary< byte, List< bool >>();
            code = new List< bool >();
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

        private List< bool > code;
        private Dictionary< byte, List< bool >> codesDictionary;
    }
}

