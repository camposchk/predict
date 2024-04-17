import os
import shutil

def organizar_imagens(pasta_origem):
    if not os.path.exists('alfanumerico'):
        os.mkdir('alfanumerico')

    mapeamento = {
        'img001': '0',
        'img002': '1',
        'img003': '2',
        'img004': '3',
        'img005': '4',
        'img006': '5',
        'img007': '6',
        'img008': '7',
        'img009': '8',
        'img010': '9',
        'img011': 'A_MAIUSCULO',
        'img012': 'B_MAIUSCULO',
        'img013': 'C_MAIUSCULO',
        'img014': 'D_MAIUSCULO',
        'img015': 'E_MAIUSCULO',
        'img016': 'F_MAIUSCULO',
        'img017': 'G_MAIUSCULO',
        'img018': 'H_MAIUSCULO',
        'img019': 'I_MAIUSCULO',
        'img020': 'J_MAIUSCULO',
        'img021': 'K_MAIUSCULO',
        'img022': 'L_MAIUSCULO',
        'img023': 'M_MAIUSCULO',
        'img024': 'N_MAIUSCULO',
        'img025': 'O_MAIUSCULO',
        'img026': 'P_MAIUSCULO',
        'img027': 'Q_MAIUSCULO',
        'img028': 'R_MAIUSCULO',
        'img029': 'S_MAIUSCULO',
        'img030': 'T_MAIUSCULO',
        'img031': 'U_MAIUSCULO',
        'img032': 'V_MAIUSCULO',
        'img033': 'W_MAIUSCULO',
        'img034': 'X_MAIUSCULO',
        'img035': 'Y_MAIUSCULO',
        'img036': 'Z_MAIUSCULO',
        'img037': 'a_minusculo',
        'img038': 'b_minusculo',
        'img039': 'c_minusculo',
        'img040': 'd_minusculo',
        'img041': 'e_minusculo',
        'img042': 'f_minusculo',
        'img043': 'g_minusculo',
        'img044': 'h_minusculo',
        'img045': 'i_minusculo',
        'img046': 'j_minusculo',
        'img047': 'k_minusculo',
        'img048': 'l_minusculo',
        'img049': 'm_minusculo',
        'img050': 'n_minusculo',
        'img051': 'o_minusculo',
        'img052': 'p_minusculo',
        'img053': 'q_minusculo',
        'img054': 'r_minusculo',
        'img055': 's_minusculo',
        'img056': 't_minusculo',
        'img057': 'u_minusculo',
        'img058': 'v_minusculo',
        'img059': 'w_minusculo',
        'img060': 'x_minusculo',
        'img061': 'y_minusculo',
        'img062': 'z_minusculo'
    }

    for filename in os.listdir(pasta_origem):
        if filename.startswith("img") and filename[3:6].isdigit():
            nome_imagem = filename[:6]

            pasta_destino = os.path.join('alfanumerico', mapeamento[nome_imagem])

            if not os.path.exists(pasta_destino):
                os.mkdir(pasta_destino)

            shutil.move(os.path.join(pasta_origem, filename), os.path.join(pasta_destino, filename))

organizar_imagens('Img')