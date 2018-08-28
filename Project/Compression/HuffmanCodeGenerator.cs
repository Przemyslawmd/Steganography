
using System.Collections.Generic;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo( "Tests" )]

namespace SteganographyCompression
{
    class HuffmanCodeGenerator
    {
        public Dictionary<byte, List< char >> CreateCodesDictionary( NodeCompress root )
        {
            codes = new List< HuffmanCode >();
            code = new HuffmanCode();
            GenerateCodes( root, true );
            return codes;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void GenerateCodes( Node node, bool bitValue )
        {
            code.AddBitToCode( bitValue );

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
                codes.Add( new HuffmanCode( node.ByteValue, code.value, code.codeLength ));
            }

            code.RemoveLastBit();
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private List< HuffmanCode > codes;
        private HuffmanCode code;
    }
}

