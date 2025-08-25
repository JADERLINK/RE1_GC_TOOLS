# RE1_GC_TOOLS

Extract only files DAT, EMD, EMG and BIN from RE1 GC;

**Translate from Portuguese Brazil**

Esses programas somente extraem os arquivos, eles não fazem o repack, considere essas tools como um bônus.

## RE1_GC_CONTAINERS_TOOL.exe

Tool dedicada a extrair os arquivos DAT, EMD, EMG, que são arquivos que têm outros arquivos dentro.
<br> Os arquivos internos não têm formatos, a tool tenta reconhecer pelo magic os arquivos BIN e TPL, os outros são definidos como UNK.
<br>A tool também extrai arquivos de extensão IIDAT e IIEMD;
<br>É gerado um arquivo de formato 'idxre1', porém o repack não foi implementado na tool.

## RE1_GC_BIN_TOOL.exe

Tool destinada a extrair os arquivos BIN, que são modelos 3D, vai ser gerado os arquivos de formatos: 'OBJ', 'SMD', 'MTL' e 'IDXMATERIAL';
<br>Nota: Morph e vertexcolor não são extraídos.

Para extrair os TPL, use o [BrawlCrate](https://github.com/soopercool101/BrawlCrate);

**At.te: JADERLINK**
<br>Thanks to "mariokart64n" and "Biohazard4X"
<br>Material information by "Albert"
<br>2025-08-24