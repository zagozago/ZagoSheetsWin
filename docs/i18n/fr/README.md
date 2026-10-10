# ZagoSheetsWin

<details>
<summary>🌐 Langues de la documentation · Choisir la langue</summary>

- [English](../../../README.md)
- [简体中文](../zh/README.md)
- [हिन्दी](../hi/README.md)
- [Español](../es/README.md)
- [العربية](../ar/README.md)
- **Français** — page actuelle
- [বাংলা](../bn/README.md)
- [Português (Brasil)](../pt/README.md)
- [Bahasa Indonesia](../id/README.md)
- [اردو](../ur/README.md)
- [Русский](../ru/README.md)
- [Deutsch](../de/README.md)

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
