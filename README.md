# emojiEdit

パソコンから au の携帯電話（フィーチャーフォン、いわゆるガラケー）に
絵文字入りメールを送信できます。

入力できる絵文字の範囲は、図柄タイプ D の一部です。
元々は、私の使っていた G'zOne CA002 の絵文字入力の代替とするべく作りましたが、
使用させていただいている絵文字データが「au/docomo共通絵文字」のため、
G'zOne CA002 で表示される図柄とは別物になります。

## 図柄タイプ D についての参考資料

https://www.au.com/ezfactory/tec/spec/3.html

## 絵文字データについて

使用させていただいている絵文字データは以下の
au（KDDI株式会社）様のサイトからダウンロードさせていただきました。
どうもありがとうございます。厚く御礼申し上げます。

絵文字データの著作権及びその他の権利は、
KDDI株式会社様および株式会社NTTドコモ様に帰属します。
取り扱いにはご注意下さいますよう、お願いいたします。

https://www.au.com/developer/android/kaihatsu/emoji_download/

## 図柄タイプ D と上記サイトでダウンロードできる絵文字のマッピングについて

以下の au（KDDI株式会社）様のサイトの
「絵文字変換表」－「au絵文字 → au/docomo共通絵文字、iPhone絵文字、他社絵文字」
を参考にさせていただきました。

https://www.au.com/mobile/service/featurephone/communication/emoji/

## ビルド環境

Visual Studio 2022 (Community)（C#）で作成しました。

※ビルド後イベントで powershell を使用して、
出力先ディレクトリにアイコンを展開しています。

## 依存するパッケージ

- BouncyCastle.1.8.1

- MimeKit.2.0.1

- MailKit.2.0.1

- System.ValueTuple.4.4.0

## その他

http://www.tkb11.com/misc/emoji-edit.php
