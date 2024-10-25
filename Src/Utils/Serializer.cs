using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace MineVote.Utils;

public class Serializer
{
    private List<byte> _data = new();

    // Writing
    public void Write(byte[] data)
    {
        _data.AddRange(data);
    }

    public void WriteString(string data)
    {
        Write(BitConverter.GetBytes(data.Length));
        Write(Encoding.UTF8.GetBytes(data));
    }

    public void WriteInt(int data) => Write(BitConverter.GetBytes(data));
    public void WriteFloat(float data) => Write(BitConverter.GetBytes(data));
    public void WriteDouble(double data) => Write(BitConverter.GetBytes(data));
    public void WriteBool(bool data) => Write(BitConverter.GetBytes(data));

    public void WriteVector2(Vector2 data)
    {
        WriteFloat(data.X);
        WriteFloat(data.Y);
    }

    public void WriteVector3(Vector3 data)
    {
        WriteFloat(data.X);
        WriteFloat(data.Y);
        WriteFloat(data.Z);
    }

    public void WriteVector4(Vector4 data)
    {
        WriteFloat(data.X);
        WriteFloat(data.Y);
        WriteFloat(data.Z);
        WriteFloat(data.W);
    }

    public void WriteRectangle(Rectangle data)
    {
        WriteInt(data.X);
        WriteInt(data.Y);
        WriteInt(data.Width);
        WriteInt(data.Height);
    }

    // Reading
    public byte[] Read(int length)
    {
        var data = _data.GetRange(0, length).ToArray();
        _data.RemoveRange(0, length);
        return data;
    }

    public string ReadString()
    {
        var length = BitConverter.ToInt32(Read(sizeof(int)));
        return Encoding.UTF8.GetString(Read(length));
    }

    public int ReadInt() => BitConverter.ToInt32(Read(sizeof(int)));
    public float ReadFloat() => BitConverter.ToSingle(Read(sizeof(float)));
    public double ReadDouble() => BitConverter.ToDouble(Read(sizeof(double)));
    public bool ReadBool() => BitConverter.ToBoolean(Read(sizeof(bool)));
    public Vector2 ReadVector2() => new(ReadFloat(), ReadFloat());
    public Vector3 ReadVector3() => new(ReadFloat(), ReadFloat(), ReadFloat());
    public Vector4 ReadVector4() => new(ReadFloat(), ReadFloat(), ReadFloat(), ReadFloat());
    public Rectangle ReadRectangle() => new(ReadInt(), ReadInt(), ReadInt(), ReadInt());

    // Utility
    public byte[] GetBytes() => _data.ToArray();
    public bool HasData() => _data.Count > 0;
    public void Clear() => _data.Clear();

    public void Save(string path)
    {
        System.IO.File.WriteAllBytes(path, GetBytes());
    }

    public void Load(string path)
    {
        Clear();
        Write(System.IO.File.ReadAllBytes(path));
    }
}