# ZagoSheetsWin

> ⚠️ **0.9.37 — Important safety update (English notice; full translation review pending).**
>
> La feuille a été ouverte dans Google Sheets. Le fichier original a été remplacé par un raccourci. Pour le récupérer, ouvrez Sauvegardes.
>
> **Conversion fidelity:** After a confirmed Google import, the original local file **may be replaced with a `.url` shortcut even when formulas, values or formatting differ**. The original bytes remain in Backups. Choose **Import as copy BEFORE opening** if you need the original to stay in the source folder. Windows **read-only** files may block replacement; in that case, the original is preserved. **New in 0.9.36:** CSV/TSV detects comma, semicolon, TAB and pipe; a per-import chooser resolves ambiguity. The bounded Windows-1252 fallback and higher text limits (60,000 logical rows / 600,000 cells) apply to new interpretations. **Backups → More actions → Reimport into a new document** repairs from the verified original backup without overwriting the previous document/shortcut. **New in 0.9.37:** the app, installer, file associations and shortcuts share one spreadsheet icon. Some text below describes an older alpha policy; use the [current English README](../../../README.md) or [Portuguese user guide](../../USER_GUIDE.md) for complete 0.9.37 behavior.


<details>
<summary>🌐 Langues de la documentation · Choisir la langue</summary>

- [EN - English](../../../README.md)
- [ZH - 中文 / Chinese (Simplified) (zh)](../zh/README.md)
- [HI - हिन्दी / Hindi (hi)](../hi/README.md)
- [ES - Español / Spanish (es)](../es/README.md)
- [AR - العربية / Arabic (ar)](../ar/README.md)
- **FR - Français / French (fr)** — page actuelle
- [BN - বাংলা / Bengali (bn)](../bn/README.md)
- [PT - Português / Portuguese (pt)](../pt/README.md)
- [ID - Bahasa Indonesia / Indonesian (id)](../id/README.md)
- [UR - اردو / Urdu (ur)](../ur/README.md)
- [RU - Русский / Russian (ru)](../ru/README.md)
- [DE - Deutsch / German (de)](../de/README.md)
- [JA - 日本語 / Japanese (ja)](../ja/README.md)
- [VI - Tiếng Việt / Vietnamese (vi)](../vi/README.md)
- [TR - Türkçe / Turkish (tr)](../tr/README.md)
- [KO - 한국어 / Korean (ko)](../ko/README.md)
- [IT - Italiano / Italian (it)](../it/README.md)
- [TH - ไทย / Thai (th)](../th/README.md)
- [TL - Filipino](../fil/README.md)
- [MS - Bahasa Melayu / Malay (ms)](../ms/README.md)
- [SW - Kiswahili / Swahili (sw)](../sw/README.md)
- [PCM - Nigerian Pidgin](../pcm/README.md)
- [MR - मराठी / Marathi (mr)](../mr/README.md)
- [TE - తెలుగు / Telugu (te)](../te/README.md)
- [HA - Hausa](../ha/README.md)
- [PA - ਪੰਜਾਬੀ / Punjabi (pa)](../pa/README.md)
- [TA - தமிழ் / Tamil (ta)](../ta/README.md)
- [YUE - 粵語 / Cantonese (yue)](../yue/README.md)
- [FA - فارسی / Persian (fa)](../fa/README.md)
- [AM - አማርኛ / Amharic (am)](../am/README.md)
- [JV - Basa Jawa / Javanese (jv)](../jv/README.md)
- [GU - ગુજરાતી / Gujarati (gu)](../gu/README.md)
- [KN - ಕನ್ನಡ / Kannada (kn)](../kn/README.md)
- [YO - Yorùbá / Yoruba (yo)](../yo/README.md)
- [BHO - भोजपुरी / Bhojpuri (bho)](../bho/README.md)
- [PS - پښتو / Pashto (ps)](../ps/README.md)
- [OR - ଓଡ଼ିଆ / Odia (or)](../or/README.md)
- [MY - မြန်မာဘာသာ / Burmese (my)](../my/README.md)
- [ML - മലയാളം / Malayalam (ml)](../ml/README.md)
- [PL - Polski / Polish (pl)](../pl/README.md)
- [SU - Basa Sunda / Sundanese (su)](../su/README.md)
- [MAI - मैथिली / Maithili (mai)](../mai/README.md)
- [UK - Українська / Ukrainian (uk)](../uk/README.md)
- [OM - Afaan Oromoo / Oromo (om)](../om/README.md)
- [UZ - Oʻzbekcha / Uzbek (uz)](../uz/README.md)
- [SD - سنڌي / Sindhi (sd)](../sd/README.md)
- [NE - नेपाली / Nepali (ne)](../ne/README.md)
- [IG - Igbo](../ig/README.md)
- [AZ - Azərbaycanca / Azerbaijani (az)](../az/README.md)
- [NL - Nederlands / Dutch (nl)](../nl/README.md)
- [GN - Avañe’ẽ / Guarani (gn)](../gn/README.md)

</details>

**Ouvrez vos fichiers de tableur locaux directement dans Google Sheets depuis Windows.**

ZagoSheetsWin est une application Windows légère qui simplifie l’ouverture d’un tableur local :

**double-cliquez sur le fichier → importez et convertissez → ouvrez dans Google Sheets**

Après une importation réussie, ZagoSheetsWin peut remplacer le fichier local d’origine par un raccourci Internet (`.url`) pointant vers le document Google Sheets, tout en conservant une sauvegarde locale récupérable du fichier original.

L’objectif est simple : faire de Google Sheets l’équivalent d’une application Windows native pour ouvrir les tableurs locaux.

## Fonctionnalités

ZagoSheetsWin relie les fichiers de tableur de Windows à Google Sheets.

Lorsque vous ouvrez un fichier local compatible, l’application peut :

- détecter et vérifier le tableur ;
- créer une sauvegarde récupérable ;
- l’importer directement dans Google Drive via les API officielles de Google ;
- le convertir en document Google Sheets natif ;
- ouvrir le tableur obtenu dans votre navigateur par défaut ;
- créer un raccourci local `.url` vers le document Google ;
- éviter un nouvel import du même fichier lors des ouvertures suivantes.

Plus besoin d’importer manuellement dans Drive, de parcourir le navigateur ou de répéter les conversions.

## Formats pris en charge

Formats visés par le développement actuel :

- `.xlsx`
- `.xls`
- `.ods`
- `.csv`
- `.tsv`

Certains formats peuvent présenter des restrictions de compatibilité supplémentaires. Les fichiers comportant des fonctionnalités qui ne peuvent pas être préservées sans risque sont traités avec prudence pour éviter toute perte silencieuse de données.

## Conçu pour Windows

ZagoSheetsWin est conçu spécifiquement pour Windows et s’intègre au système d’exploitation grâce à :

- **Ouvrir avec**
- l’enregistrement des types de fichiers
- l’ouverture des fichiers par double-clic
- une intégration facultative à l’Explorateur de fichiers
- un programme d’installation Windows natif

L’application ne modifie pas silencieusement vos applications Windows par défaut. Vous gardez le contrôle des associations de fichiers.

## La sécurité dès la conception

ZagoSheetsWin considère le remplacement d’un fichier local comme une opération récupérable.

Avant de retirer un fichier original de son dossier, l’application vérifie que :

1. une sauvegarde récupérable existe ;
2. le document Google Sheets a été créé avec succès ;
3. l’association locale a été enregistrée durablement ;
4. le raccourci Internet a été écrit et validé.

Si le processus échoue, le fichier original est conservé.

Une règle simple guide l’application :

> Ne jamais détruire des données utilisateur sans avertissement.

## Sauvegardes

Les fichiers originaux peuvent être enregistrés dans un espace de sauvegarde local privé avant d’être remplacés par des raccourcis.

La gestion des sauvegardes comprend des limites de stockage configurables, des politiques de conservation et des options de nettoyage.

Les sauvegardes protègent le fichier original importé. Elles ne constituent **pas une synchronisation bidirectionnelle** : les modifications effectuées ensuite dans Google Sheets ne sont pas répercutées dans le fichier de tableur d’origine.

## Confidentialité et accès à Google

ZagoSheetsWin communique directement avec les API Google depuis votre ordinateur.

- Aucun contenu de vos feuilles de calcul n’est envoyé à un serveur Zagotools.
- Les jetons OAuth sont conservés localement et protégés par les mécanismes de sécurité de Windows.
- L’application utilise le champ d’autorisation Google Drive `drive.file`, qui limite l’accès aux fichiers créés ou ouverts via l’application.
- Aucun outil d’analyse n’est nécessaire au processus d’importation.

Politique de confidentialité et conditions d’utilisation :

https://zagotools.top/legal.html

## État du projet

ZagoSheetsWin est en développement actif et doit actuellement être considéré comme un **logiciel en version alpha**.

Le parcours principal Windows → Google Sheets fonctionne et continue de s’améliorer sur l’installation, la récupération, la compatibilité des formats, l’internationalisation et l’expérience utilisateur.

Des changements sont possibles avant la première version stable.

## Relation avec Open in Google

ZagoSheetsWin est basé sur [Open in Google](https://github.com/SwatiK425/open-in-google), créé par [SwatiK425](https://github.com/SwatiK425), et en est dérivé.

Open in Google a fourni les fondations initiales et l’inspiration du projet.

ZagoSheetsWin est depuis devenu une application Windows indépendante, avec sa propre architecture, son installateur, son interface, son système de sauvegarde et de récupération, ses associations de fichiers, sa gestion des formats et son parcours d’ouverture des fichiers locaux dans Google Sheets.

Le projet d’origine reste indépendant. Des améliorations génériques pourront lui être proposées lorsque cela sera pertinent, tandis que ZagoSheetsWin poursuivra son évolution autonome.

## Code source ouvert

ZagoSheetsWin est un logiciel gratuit et open source.

Le projet respecte les obligations d’attribution et de licence du code original d’Open in Google, tout en identifiant clairement les développements ultérieurs de ZagoSheetsWin / Zagotools.

Voir :

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## Licence

Licence MIT.

Consultez [LICENSE](../../../LICENSE) pour plus de détails.

---

**ZagoSheetsWin — un projet Zagotools**

De petits logiciels pour de vrais problèmes.
