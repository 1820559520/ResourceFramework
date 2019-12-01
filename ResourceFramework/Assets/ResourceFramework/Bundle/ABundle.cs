﻿using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ResourceFramework
{
    internal abstract class ABundle
    {
        /// <summary>
        /// AssetBundle
        /// </summary>
        internal AssetBundle assetBundle { get; set; }

        /// <summary>
        /// bundle url
        /// </summary>
        internal string url { get; set; }

        /// <summary>
        /// 引用计数器
        /// </summary>
        internal int reference { get; set; }

        //bundle是否加载完成
        internal bool done { get; set; }

        /// <summary>
        /// 加载bundle
        /// </summary>
        internal abstract void Load();

        /// <summary>
        /// 卸载bundle
        /// </summary>
        internal abstract void UnLoad();

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="name">资源名称</param>
        /// <returns>指定名字的资源</returns>
        internal abstract Object LoadAsset(string name);

        /// <summary>
        /// 增加引用
        /// </summary>
        internal void AddReference()
        {
            //自己引用+1
            ++reference;
        }

        /// <summary>
        /// 减少引用
        /// </summary>
        internal void ReduceReference()
        {
            //自己引用-1
            --reference;

            if (reference < 0)
            {
                throw new System.Exception($"{GetType()}.{nameof(ReduceReference)}() less than 0,{nameof(url)}:{url}.");
            }

            //严格遵循要加载完了才能释放
            if (!done)
            {
                return;
            }

            //引用为0,直接释放
            if (reference == 0)
            {
                UnLoad();
            }
        }
    }
}