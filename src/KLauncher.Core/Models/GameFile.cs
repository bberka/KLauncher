using Ardalis.Result;
using KLauncher.Shared.Abstract;
using Microsoft.VisualBasic.CompilerServices;

namespace KLauncher.Core.Models;

public class GameFile : IEquatable<GameFile>
{

    public static Result<GameFile> ReadFromStream(string fullFilePath) {
        var 
    }
    
    private GameFile(string pathFromRoot, string hash, long size, long lastUpdate, bool isForceCheck) {
        PathFromRoot = pathFromRoot;
        Hash = hash;
        Size = size;
        LastUpdate = lastUpdate;
        IsForceCheck = isForceCheck;
    }
    public string PathFromRoot { get; }
    public string PathHash => Conversions.ToString(PathFromRoot.GetHashCode());
    public string Hash { get; }
    public long Size { get; }
    public long LastUpdate { get; }
    public bool IsForceCheck { get; }


    public static bool operator ==(GameFile gameFile, GameFile gameFile2) {
        return Equals(gameFile, gameFile2);
    }
    public static bool operator !=(GameFile gameFile, GameFile gameFile2){
        return !Equals(gameFile, gameFile2);
    }
    public bool Equals(GameFile? other) {
        if (ReferenceEquals(null, other)) return false;
        return other.Hash == this.Hash;
    }

    public override bool Equals(object? obj) {
        if (ReferenceEquals(null, obj)) return false;
        if (obj.GetType() != GetType()) return false;
        return ((GameFile)obj).Hash == this.Hash;
    }

    public override int GetHashCode() {
        return HashCode.Combine(LastUpdate, PathFromRoot, Hash, Size, IsForceCheck);
    }
}