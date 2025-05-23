using System.IO;

namespace Unity.Multiplayer.Playmode.VirtualProjects.Editor
{
    class PlayStateChangeMessage
    {
        static void Serialize(BinaryWriter writer, object value)
        {
        }

        static object Deserialize(BinaryReader reader)
        {
            return new PlayStateChangeMessage();
        }

        [SerializeMessageDelegates] // ReSharper disable once UnusedMember.Global
        public static SerializeMessageDelegates SerializeMethods() => new SerializeMessageDelegates { SerializeFunc = Serialize, DeserializeFunc = Deserialize };
    }
}
