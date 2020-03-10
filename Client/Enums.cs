using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    enum InputType
    {
        File,
        Byte,
        BinarySequence
    }

    enum CodingMode
    {
        kod1,
        kod2,
        odkod1,
        odkod2
    }

    enum OutputType
    {
        File, 
        Console
    }

    enum OutputStyle
    {
        Binary,
        TextualBitsSeq
    }
}
