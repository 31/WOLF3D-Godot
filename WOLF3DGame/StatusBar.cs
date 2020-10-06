﻿using System.Collections.Generic;
using Godot;
using System.Linq;
using System.Xml.Linq;
using System.Collections;
using System;
using WOLF3D.WOLF3DGame.OPL;

namespace WOLF3D.WOLF3DGame
{
    public class StatusBar : Viewport, IDictionary<string, StatusNumber>, ICollection<KeyValuePair<string, StatusNumber>>, IEnumerable<KeyValuePair<string, StatusNumber>>, IEnumerable, IDictionary, ICollection, IReadOnlyDictionary<string, StatusNumber>, IReadOnlyCollection<KeyValuePair<string, StatusNumber>>
    {
        public XElement XML { get; set; }
        public StatusBar() : this(Assets.XML?.Element("VgaGraph")?.Element("StatusBar")) { }
        public StatusBar(XElement xml)
        {
            Name = "StatusBar";
            Disable3d = true;
            RenderTargetClearMode = ClearMode.OnlyNextFrame;
            RenderTargetVFlip = true;
            XML = xml;
            ImageTexture pic = Assets.PicTextureSafe(XML?.Attribute("Pic")?.Value);
            Size = pic?.GetSize() ?? Vector2.Zero;
            if (pic != null)
                AddChild(new Sprite()
                {
                    Name = "StatusBarPic",
                    Texture = pic,
                    Position = Size / 2f,
                });
            foreach (XElement number in XML?.Elements("Number") ?? Enumerable.Empty<XElement>())
                Add(new StatusNumber(number));
        }

        private readonly Dictionary<string, StatusNumber> StatusNumbers = new Dictionary<string, StatusNumber>();
        public void Add(StatusNumber statusNumber) => Add(statusNumber.Name, statusNumber);

        public void Add(string key, StatusNumber value)
        {
            AddChild(value);
            StatusNumbers.Add(key, value);
        }

        public void Clear()
        {
            foreach (StatusNumber statusNumber in Values)
                RemoveChild(statusNumber);
            StatusNumbers.Clear();
        }

        public void Remove(string key) => TryRemove(key);

        public bool TryRemove(string key)
        {
            if (StatusNumbers.TryGetValue(key, out StatusNumber statusNumber))
            {
                RemoveChild(statusNumber);
                return StatusNumbers.Remove(key);
            }
            return false;
        }

        bool IDictionary<string, StatusNumber>.Remove(string key) => TryRemove(key);

        public void Add(KeyValuePair<string, StatusNumber> item) => Add(item.Key, item.Value);

        public bool Remove(KeyValuePair<string, StatusNumber> item) => TryRemove(item.Key);

        public void Add(object key, object value)
        {
            if (key is string @string && value is StatusNumber statusNumber)
                Add(@string, statusNumber);
        }

        public void Remove(object key)
        {
            if (key is string @string)
                TryRemove(@string);
        }

        public object this[object key]
        {
            get => ((IDictionary)StatusNumbers)[key];
            set => Add(key, value);
        }

        public StatusNumber this[string key]
        {
            get => ((IDictionary<string, StatusNumber>)StatusNumbers)[key];
            set => Add(key, value);
        }

        public StatusNumber.Stat? Stat(string key) => TryGetValue(key, out StatusNumber statusNumber) ? statusNumber.GetStat() : (StatusNumber.Stat?)null;

        public IEnumerable<StatusNumber.Stat> GetStats()
        {
            foreach (StatusNumber statusNumber in StatusNumbers.Values)
                yield return statusNumber.GetStat();
        }

        public IEnumerable<StatusNumber.Stat> NextLevelStats()
        {
            foreach (StatusNumber statusNumber in StatusNumbers.Values)
                yield return statusNumber.GetNextLevelStat();
        }

        public StatusBar Set(IEnumerable<StatusNumber.Stat> stats)
        {
            foreach (StatusNumber.Stat stat in stats)
                if (TryGetValue(stat.Name, out StatusNumber statusNumber))
                    statusNumber.Set(stat);
            return this;
        }

        public bool Conditional(XElement xml)
        {
            if (!ConditionalOne(xml))
                return false;
            foreach (XElement conditional in xml?.Elements("Conditional") ?? Enumerable.Empty<XElement>())
                if (!ConditionalOne(conditional))
                    return false;
            return true;
        }

        public bool ConditionalOne(XElement xml) =>
            xml?.Attribute("If")?.Value is string stat
                && !string.IsNullOrWhiteSpace(stat)
                && TryGetValue(stat, out StatusNumber statusNumber)
            ? (
            (
            uint.TryParse(xml?.Attribute("Equals")?.Value, out uint equals)
                    ? statusNumber.Value == equals : true
            )
            &&
            (
            uint.TryParse(xml?.Attribute("LessThan")?.Value, out uint less)
                    ? statusNumber.Value < less : true
            )
            &&
            (
            uint.TryParse(xml?.Attribute("GreaterThan")?.Value, out uint greater)
                    ? statusNumber.Value > greater : true
            )
            &&
            (
            uint.TryParse(xml?.Attribute("MaxEquals")?.Value, out uint maxEquals)
                    ? statusNumber.Max == maxEquals : true
            )
            &&
            (
            uint.TryParse(xml?.Attribute("MaxLessThan")?.Value, out uint maxLess)
                    ? statusNumber.Max < maxLess : true
            )
            &&
            (
            uint.TryParse(xml?.Attribute("MaxGreaterThan")?.Value, out uint maxGreater)
                    ? statusNumber.Max > maxGreater : true
            )
            ) : true;

        public StatusBar Effect(XElement xml)
        {
            EffectOne(xml);
            foreach (XElement effect in xml?.Elements("Effect") ?? Enumerable.Empty<XElement>())
                EffectOne(effect);
            return this;
        }

        public StatusBar EffectOne(XElement xml)
        {
            if (xml?.Attribute("SetMaxOf")?.Value is string setMaxOfString
                && !string.IsNullOrWhiteSpace(setMaxOfString)
                && TryGetValue(setMaxOfString, out StatusNumber setMaxOf)
                && uint.TryParse(xml?.Attribute("SetMax")?.Value, out uint setMax))
                setMaxOf.Max = setMax;
            if (xml?.Attribute("AddToMaxOf")?.Value is string addToMaxString
                && !string.IsNullOrWhiteSpace(addToMaxString)
                && TryGetValue(addToMaxString, out StatusNumber addToMaxOf)
                && uint.TryParse(xml?.Attribute("AddToMax")?.Value, out uint addToMax))
                addToMaxOf.Max = addToMax;
            if (xml?.Attribute("SetTo")?.Value is string setString
                && !string.IsNullOrWhiteSpace(setString)
                && TryGetValue(setString, out StatusNumber setStatusNumber)
                && uint.TryParse(xml?.Attribute("Set")?.Value, out uint set))
                setStatusNumber.Value = set;
            if (xml?.Attribute("AddTo")?.Value is string stat
                && !string.IsNullOrWhiteSpace(stat)
                && TryGetValue(stat, out StatusNumber statusNumber)
                && uint.TryParse(xml?.Attribute("Add")?.Value, out uint add))
                statusNumber.Value += add;
            SoundBlaster.Play(xml);
            return this;
        }

        #region IDictionary boilerplate
        public bool ContainsKey(string key) => ((IDictionary<string, StatusNumber>)StatusNumbers).ContainsKey(key);
        public bool TryGetValue(string key, out StatusNumber value) => ((IDictionary<string, StatusNumber>)StatusNumbers).TryGetValue(key, out value);
        public bool Contains(KeyValuePair<string, StatusNumber> item) => ((ICollection<KeyValuePair<string, StatusNumber>>)StatusNumbers).Contains(item);
        public void CopyTo(KeyValuePair<string, StatusNumber>[] array, int arrayIndex) => ((ICollection<KeyValuePair<string, StatusNumber>>)StatusNumbers).CopyTo(array, arrayIndex);
        public IEnumerator<KeyValuePair<string, StatusNumber>> GetEnumerator() => ((IEnumerable<KeyValuePair<string, StatusNumber>>)StatusNumbers).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)StatusNumbers).GetEnumerator();
        public bool Contains(object key) => ((IDictionary)StatusNumbers).Contains(key);
        IDictionaryEnumerator IDictionary.GetEnumerator() => ((IDictionary)StatusNumbers).GetEnumerator();
        public void CopyTo(Array array, int index) => ((ICollection)StatusNumbers).CopyTo(array, index);

        public ICollection<string> Keys => ((IDictionary<string, StatusNumber>)StatusNumbers).Keys;

        public ICollection<StatusNumber> Values => ((IDictionary<string, StatusNumber>)StatusNumbers).Values;

        public int Count => ((ICollection<KeyValuePair<string, StatusNumber>>)StatusNumbers).Count;

        public bool IsReadOnly => ((ICollection<KeyValuePair<string, StatusNumber>>)StatusNumbers).IsReadOnly;

        ICollection IDictionary.Keys => ((IDictionary)StatusNumbers).Keys;

        ICollection IDictionary.Values => ((IDictionary)StatusNumbers).Values;

        public bool IsFixedSize => ((IDictionary)StatusNumbers).IsFixedSize;

        public object SyncRoot => ((ICollection)StatusNumbers).SyncRoot;

        public bool IsSynchronized => ((ICollection)StatusNumbers).IsSynchronized;

        IEnumerable<string> IReadOnlyDictionary<string, StatusNumber>.Keys => ((IReadOnlyDictionary<string, StatusNumber>)StatusNumbers).Keys;

        IEnumerable<StatusNumber> IReadOnlyDictionary<string, StatusNumber>.Values => ((IReadOnlyDictionary<string, StatusNumber>)StatusNumbers).Values;
        #endregion IDictionary boilerplate
    }
}
