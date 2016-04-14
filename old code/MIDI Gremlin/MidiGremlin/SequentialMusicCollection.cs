using System;
using System.Collections;
using System.Collections.Generic;

namespace MidiGremlin
{
    ///<summary>
    ///The class SequentialMusicCollection inherits from the class MusicObject.
    ///The class consist of a list of MusicObjects, which will be played apart from each other.
    ///The MusicObjects will be played in a certain order.
    ///</summary>
    public class SequentialMusicCollection : MusicObject, IList<MusicObject>
    {
        public SequentialMusicCollection (IEnumerable<MusicObject> children)
        {
            throw new NotImplementedException();
        }
        public SequentialMusicCollection (params MusicObject[] children)
        {
            throw new NotImplementedException();
        }

        public MusicObject this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsReadOnly
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Add (MusicObject item)
        {
            throw new NotImplementedException();
        }

        public void Clear ()
        {
            throw new NotImplementedException();
        }

        public bool Contains (MusicObject item)
        {
            throw new NotSupportedException();
        }

        public void CopyTo (MusicObject[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<MusicObject> GetEnumerator ()
        {
            throw new NotImplementedException();
        }

        public int IndexOf (MusicObject item)
        {
            throw new NotImplementedException();
        }

        public void Insert (int index, MusicObject item)
        {
            throw new NotImplementedException();
        }

        public bool Remove (MusicObject item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt (int index)
        {
            throw new NotImplementedException();
        }

        internal override IEnumerator<SingleBeat> GetChildren ()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            throw new NotImplementedException();
        }
    }
}