
using System.Collections.Generic;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo( "Tests" )]

namespace SteganographyCompression
{
    class HuffmanCodes
    {
        public Dictionary<byte, List<char>> CreateCodesDictionary( NodeCompress root )
        {
            codes = new Dictionary< byte, List< char >>();
            code = new List< char >();
            GenerateCodes( root, '1' );
            return codes;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void GenerateCodes( Node node, char token )
        {
            code.Add( token );

            if ( node.Left != null )
            {
                GenerateCodes( node.Left, '0' );
            }
            if ( node.Right != null )
            {
                GenerateCodes( node.Right, '1' );
            }
            else
            {
                codes.Add( node.ByteValue, new List< char >( code ) );
            }

            code.RemoveAt( code.Count - 1 );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private List< char > code;
        private Dictionary< byte, List< char >> codes;
    }
}

