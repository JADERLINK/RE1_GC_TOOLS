using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RE1_GC_BIN_TOOL.ALL;

namespace RE1_GC_BIN_TOOL.EXTRACT
{
    public class RE1GCBIN
    {
        public Re1GcBinHeader Header;

        public (short vx, short vy, short vz, byte color_index, byte weightmap_index)[] Vertex_Position_Array;
        public (short nx, short ny, short nz, byte color_index, byte weightmap_index)[] Vertex_Normal_Array;
        public (short tu, short tv)[] Vertex_UV_Array;

        public Bone[] Bones;
        public WeightMap[] WeightMaps;
        public MaterialBin[] Materials;
    }


    public class MaterialBin
    {
        public MaterialPart material;

        public (FaceIndex i1, FaceIndex i2, FaceIndex i3)[] face_index_array;
    }

    public struct FaceIndex
    {
        public ushort indexVertex;
        public ushort indexNormal;
        public ushort indexUV;
    }

    public struct WeightMap
    {
        public byte boneId1;
        public byte boneId2;
        public byte boneId3;
        public byte count;
        public byte weight1;
        public byte weight2;
        public byte weight3;
        public byte unused;
    }

    public struct Bone
    {
        public byte BoneID;
        public byte BoneParent;
        public float PositionX;
        public float PositionY;
        public float PositionZ;
    }


    public class Re1GcBinHeader
    {
        public uint bone_offset; // magic: 0x00_00_00_40
        public uint unknown_x04;
        public uint unknown_x08;
        public uint vertex_colour_offset; //colors

        public uint vertex_texcoord_offset; // UV
        public uint weightmap_offset;
        public byte weightmap_count;
        public byte bone_count;
        public ushort material_count;
        public uint material_offset;

        public uint Bin_flags;
        public uint Tex_count;
        public byte vertex_scale;
        public byte unknown_x29;
        public ushort weightmap2_count; // unused
        public uint morph_offset; // não implementado

        public uint vertex_position_offset;
        public uint vertex_normal_offset;
        public ushort vertex_position_count;
        public ushort vertex_normal_count;
        public uint version_flags;
    }


}
