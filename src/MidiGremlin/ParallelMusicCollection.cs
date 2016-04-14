using System;
using System.Collections;
using System.Collections.Generic;
using MidiGremlin.Internal;

namespace MidiGremlin
{
    public class ParallelMusicCollection : MusicObject, IList<MusicObject>
    {
        public ParallelMusicCollection (IEnumerable<MusicObject> children)
        {
            throw new NotImplementedException();
        }
        public ParallelMusicCollection (params MusicObject[] children)
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

        internal override IEnumerable<SingleBeat> GetChildren (Instrument playedBy, int startTime)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            throw new NotImplementedException();
        }
    }
}