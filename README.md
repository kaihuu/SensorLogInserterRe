# SensorLogInserterRe
���s���O��DB�Ɋi�[���邽�߂̃v���O�����B  
��X��ECOLOG������s����B  

## 2018�tLEAF�����p�g����
�ʂ�**�ό��I�v�V����**��p�����������s�����ꍇ�ɂ́A��q����ό��C���T�[�^�[�̏����̍��ڂɋL�ڂ���Ă���ύX���s���B  

**TODO**
**���H�����N�f�[�^��DB�ɓo�^������K�v������**

1. �N���㌤���������o�[�Ƀ`�F�b�N
2. ���Ԏw�肠��Ƀ`�F�b�N�A�J�����_�[��0424~0425��I������
3. ���胂�f����GPS�␳���w��
4. �ό��I�v�V�����Ƀ`�F�b�N
5. �f�[�^�}���{�^�����N���b�N

�ȏ�̎菇�ŃC���T�[�g���s����B  

## �ό��C���T�[�^�[�̏���
- itsserver�̌l�f�B���N�g���ɉ^�]���O�̃A�b�v���[�h
- NMEA�f�[�^��ECOLOGDB�ւ̃A�b�v���[�h
- ECOLOGDB��PLACES�e�[�u���ւ̃��R�[�h�̑}��
- �g�p�����Ԃ�DB�ƃR�[�h�ɒǉ�
- �}�b�v�}�b�`���O�p���H�����N�̒ǉ�

### itsserver�̌l�f�B���N�g���ɉ^�]���Ocsv�̃A�b�v���[�h
`\\itsserver\ECOLOG_LogData_itsserver\[DriverName]\[CarName]\[SensorName]`�ȉ��̃f�B���N�g���Ɉȉ��̃t�@�C�����A�b�v���[�h  
- [������]MetaDataLog
- [������]Unsent16HzAccel
- [������]UnsentGPS
- [������]UnsentIlluminance
- [������]UnsentNmea

**TODO*
�e�t�@�C���̏ڍ�  

### NMEA�f�[�^��ECOLOGDB�փA�b�v���[�h
`NMEAInserter`�𗘗p����B  
**TODO**
NMEAInserter�ɂ��Ă̋L�q�B  

### ECOLOGDB��PLACES�e�[�u���ւ̃��R�[�h�}��
�h���C�o�[��サ���n�_�A���Ԃ����n�_�̍��W��DB�ɑ}������B  
���̍ہAPROPERTY='sightseeing'��ݒ肷��B  

### �g�p�����Ԃ�DB�ƃR�[�h�ɒǉ�
**TODO**
�R�[�h������DB��Cars�e�[�u�����Q�Ƃ���悤�ɂ���DB�ƃR�[�h�̏d���𖳂����B  

Cars�e�[�u���Ɏg�p�����Ԃ̏���o�^����B  
TIRE_RADIUS�̓��[�g���P�ʂ̃^�C�����a���L�ڂ���B  
(�z�C�[�����a�ł͂Ȃ����Ƃɒ��ӂ��K�v�B)  
[���̃y�[�W�̏��](http://cars-japan.net/index.html)�̏����Q�l�ɂ���B  

FRONTAL_PROJECTED_AREA�͑O�����e�ʐς������B  
ECOLOGDB�ł̌v�Z���͈ȉ��̒ʂ�ł���B  
`�ԕ� x (�ԍ� - �^�C�����a)`  
�ȏ�̎��ŋ��߂��l�̏�����3�ʂ��l�̌ܓ������l��}������B  

CD_VALUE�ɂ͋�C��R�W����}������B  
�J�^���O�X�y�b�N�ɋL�ڂ��������肷��炵���B  

### �}�b�v�}�b�`���O�p�̓��H�����N�̒ǉ�
���y��ʏȂ����J���Ă�������_�������m�[�h�Ɠ��H�`����������H�����N�\���_�ɂ��\�������f�[�^��}������K�v������B  

**TODO**
�f�[�^�}�����@�̋L�q�B  