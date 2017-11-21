Feature: Project Applicable Regulations Questionnaire
    As a project member
    I want to be able to start and finish a questionnaire to find out applicable regulations for the project

Background:
    Given I am signed in as user and on first page
        And I click on the element "h4=Test title"
        And I click on the element "a=Work packages"
        And I click on the element "span=Medical need and product specification"
        And I click on the element "span=Regulation checklist"

Scenario: Take questionnaire and answer all questions YES
    When I click on the element "button=Take questionnaire"
        And I answer YES to question "1. Is your device “implantable” and “not active”?"
        And I answer YES to question "2. Is your device “active” and its source of energy is electrical?"
        And I answer YES to question "2.1. Is your device a software or does it contain software (applies also to firmware)?"
        And I answer YES to question "2.1.1. Is the device containing SW intended to be part of a IT-network?"
        And I answer YES to question "3. Is your device “implantable” and “active”?"
        And I answer YES to question "4. Is your device intended to be sterile?"
        And I answer YES to question "4.1. Will it be terminally sterilized?"
        And I answer YES to question "4.1.1. (Will it be terminally sterilized) by Ethilene Oxide?"
        And I answer YES to question "4.1.2. (Will it be terminally sterilized) by Irradiation?"
        And I answer YES to question "4.1.3. (Will it be terminally sterilized) by moist heat, steam?"
        And I answer YES to question "4.1.4. (Will it be terminally sterilized) by dry heat?"
        And I answer YES to question "4.2. Will it be sterilized in process?"
        And I answer YES to question "5. Does your device come in contact to the human body (any kind of contact, brief or permanent, in any location of the human body)?"
        And I answer YES to question "5.1. Does your device contain substances of animal origin?"
    Then I expect the element "a=EN ISO 14630:2012" is visible
        And I expect the element "a=EN ISO 16061: 2015" is visible 
        And I expect the element "a=IEC 60601-1:2005+AMD1:2012 CSV (consolidated version)" is visible
        And I expect the element "a=EN 62304:2006+A1:2015" is visible
        And I expect the element "a=BS EN 80001-1:2011 and PD IEC/TR 80001-2-1:2012" is visible
        And I expect the element "a=ISO 14708-1:2014" is visible
        And I expect the element "a=EN ISO 11607-1:2009+A1:2014" is visible
        And I expect the element "a=ISO 14644-1" is visible
        And I expect the element "a=EN ISO 11135:2014" is visible
        And I expect the element "a=EN ISO 11138-2:2017" is visible
        And I expect the element "a=ISO 11137" is visible
        And I expect the element "a=ISO 17665-1:2006(en)" is visible
        And I expect the element "a=ISO 20857:2010(en)" is visible
        And I expect the element "a=ISO 13408-1:2008(en)" is visible
        And I expect the element "a=ISO 10993-1" is visible
        And I expect the element "a=ISO 22442-1:2015(en)" is visible
        And I expect the element "a=EN ISO 13485:2016" is visible
        And I expect the element "a=EN ISO 14971:2012" is visible
        And I expect the element "a=http://ec.europa.eu/docsroom/documents/17522/attachments/1/translations/en/renditions/native" is visible
        And I expect the element "a=IEC 62366-1" is visible
        And I expect the element "a=EN ISO 15223-1:2016" is visible

Scenario: Take questionnaire and answer all questions NO
    Then I expect the element "h3=Last results:" is visible
    When I click on the element "button=Take questionnaire"
        And I click on the element "button=No"
        And I click on the element "button=No"
    Then I expect the question "3. Is your device “implantable” and “active”?" is visible
    When I click on the element "button=No"
        And I click on the element "button=No"
    Then I expect the question "5. Does your device come in contact to the human body (any kind of contact, brief or permanent, in any location of the human body)?" is visible
    When I click on the element "button=No"
        And I expect the element "a=EN ISO 14971:2012" is visible

Scenario: Take questionnaire but stop it
    Then I expect the element "h3=Previous results:" is visible
    When I click on the element "button=Take questionnaire"
        And I go back to last page
    Then I expect the element "a=Continue answering" is visible