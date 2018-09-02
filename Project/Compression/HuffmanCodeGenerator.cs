
using System.Collections.Generic;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo( "Tests" )]

namespace SteganographyCompression
{
    class HuffmanCodeGenerator
    {
        public Dictionary< byte, HuffmanCode > CreateCodesDictionary( NodeCompress root )
        {
            codesDictionary = new Dictionary< byte, HuffmanCode >();
            code = new HuffmanCode();
            GenerateCodes( root, true );
            return codesDictionary;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void GenerateCodes( Node node, bool token )
        {
            IncrementCode( token );

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
                codesDictionary.Add( node.ByteValue, new HuffmanCode( code ));
            }

            DecrementCode();
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void IncrementCode( bool token )
        {
            byte codePart = ( code.tokens.Count == 0 ) ? (byte) 0 : code.tokens.Pop();

            if ( token )
            {
                codePart |= (byte) ( 1 << ( 7 - ( code.length % BitsInByte )));
            }
            else
            {
                codePart &= (byte) ~( 1 << ( 7 - ( code.length % BitsInByte )));
            }

            code.tokens.Push( codePart );
            code.length++;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void DecrementCode()
        {
            if ( code.length % BitsInByte == 1 )
            {
                code.tokens.Pop();
            }

            code.length--;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private Dictionary< byte, HuffmanCode > codesDictionary;
        private HuffmanCode code;
        private readonly int BitsInByte = 8;
    }
}

