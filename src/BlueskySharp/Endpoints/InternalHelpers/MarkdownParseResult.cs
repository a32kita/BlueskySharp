using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BlueskySharp.Endpoints.InternalHelpers
{
    internal class MarkdownParseResult
    {
        public string ParsedText { get; set; }

        public Facet[] Facets { get; set; }

        public static MarkdownParseResult ParseMarkdownToFacets(string markdownText)
        {
            if (string.IsNullOrEmpty(markdownText))
            {
                return new MarkdownParseResult
                {
                    ParsedText = string.Empty,
                    Facets = Array.Empty<Facet>()
                };
            }

            // マークダウン形式のリンク正規表現: [テキスト](リンク)
            var linkRegex = new Regex(@"\[([^\]]+)\]\((https?://[^\)]+)\)", RegexOptions.Compiled);
            var matches = linkRegex.Matches(markdownText);

            var facets = new List<Facet>();
            var parsedTextBuilder = new StringBuilder();
            int currentTextByteOffset = 0;

            var encoding = Encoding.UTF8;

            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    // リンク情報を抽出
                    string linkText = match.Groups[1].Value; // "テキスト"
                    string linkUrl = match.Groups[2].Value;  // "URL"

                    // マッチ前までの部分を結果テキストに追加
                    string textBeforeMatch = markdownText.Substring(
                        currentTextByteOffset,
                        match.Index - currentTextByteOffset
                    );
                    parsedTextBuilder.Append(textBeforeMatch);

                    // 現在の解析済みテキストまでのバイト数
                    int byteStart = encoding.GetByteCount(parsedTextBuilder.ToString());

                    // マッチしたリンクのテキスト部分を結果テキストに追加
                    parsedTextBuilder.Append(linkText);

                    // 現在の解析済みテキストまでのバイト数
                    int byteEnd = encoding.GetByteCount(parsedTextBuilder.ToString());

                    // Facet 作成
                    facets.Add(new Facet
                    {
                        Index = new FacetIndex
                        {
                            ByteStart = byteStart,
                            ByteEnd = byteEnd
                        },
                        Features = new[]
                        {
                        new Feature
                        {
                            Type = "app.bsky.richtext.facet#link",
                            Uri = new Uri(linkUrl)
                        }
                    }
                    });

                    // オフセットを更新
                    currentTextByteOffset = match.Index + match.Length;
                }
            }

            // 残りの部分を追加
            if (currentTextByteOffset < markdownText.Length)
            {
                parsedTextBuilder.Append(markdownText.Substring(currentTextByteOffset));
            }

            return new MarkdownParseResult
            {
                ParsedText = parsedTextBuilder.ToString(),
                Facets = facets.ToArray()
            };
        }
    }
}

